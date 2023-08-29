using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using System.Text.Json;
using System.Linq;
using System.Reflection;

namespace QuizzBankBE.Services.ScoreServices
{
    public class ScoreServicesImpl : IScoreServicesImpl
    {
        public static DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public ScoreServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public ScoreServicesImpl()
        {
        }

        public async Task<ServiceResponse<AllQuizzResponseDTO>> GetScore(int accessID)
        {
            var servicesResponse = new ServiceResponse<AllQuizzResponseDTO>();
            var doQuizResponseDTO = new AllQuizzResponseDTO();

            var dbQuizzResponse = (from a in _dataContext.QuizAccesses
                                   join u in _dataContext.Users on a.UserId equals u.Id
                                   join q in _dataContext.Quizzes on a.QuizId equals q.Id
                                   join c in _dataContext.Courses on q.CourseId equals c.Id
                                   where a.Id == accessID
                                   select new
                                   {
                                       quizzAccess = _mapper.Map<QuizAccessDTO>(a),
                                       userDoQuizz = _mapper.Map<UserDTO>(u),
                                       quiz = _mapper.Map<QuizDTO>(q),
                                       course = _mapper.Map<CourseDTO>(c)
                                   }).FirstOrDefault();

            doQuizResponseDTO.quiz = dbQuizzResponse.quiz;
            doQuizResponseDTO.quizzAccess = dbQuizzResponse.quizzAccess;
            doQuizResponseDTO.userDoQuizz = dbQuizzResponse.userDoQuizz;
            doQuizResponseDTO.course = dbQuizzResponse.course;

            var quizResult = (from qr in _dataContext.QuizResponses
                              join ques in _dataContext.Questions on qr.QuestionId equals ques.Id
                              join qa in _dataContext.QuestionAnswers on ques.Id equals qa.QuestionId into qaGroup
                              from qag in qaGroup.DefaultIfEmpty()
                              join qm in _dataContext.MatchSubQuestions on ques.Id equals qm.QuestionId into qmGroup
                              from qmg in qmGroup.DefaultIfEmpty()
                              where qr.AccessId == accessID                       
                              select new {qr, ques, qag, qmg}
                             ).AsEnumerable().GroupBy(i => new { i.qr, i.ques }).Distinct().Select( i => new
                             {
                                   QuizzResponse = _mapper.Map<Do1QuizResponseDTO>(i.Key.qr),
                                   Question = _mapper.Map<GeneralQuestionResultDTO>(i.Key.ques),
                                   QuestionAnswer = i.Select(qa => _mapper.Map<QuestionAnswerResultDTO>(qa.qag)).ToList(),
                                   MatchSubQuestion = i.Select(qm => _mapper.Map<MatchSubQuestionResponseDTO>(qm.qmg)).ToList()
                             });

            foreach (var item in quizResult)
            {
                item.QuizzResponse.AnswerToJson = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(item.QuizzResponse?.Answer);
                doQuizResponseDTO.questionReults.Add(item);
                doQuizResponseDTO.totalPoint += item.QuizzResponse.Mark;
            }

            doQuizResponseDTO.status = doQuizResponseDTO.totalPoint >= doQuizResponseDTO.quiz.PointToPass ? "Pass" : "Failed";

            servicesResponse.Data = doQuizResponseDTO;
            servicesResponse.Message = "OK";

            return servicesResponse;
        }

        public async Task<ServiceResponse<float>> doQuestion<T>(T doQuestionDTO) where T : DoQuestionDTO
        {
            var servicesResponse = new ServiceResponse<float>();
            var scoreSvcs = new ScoreServicesImpl();
            var scoreSvcType = scoreSvcs.GetType();

            var questionType = doQuestionDTO.GetType();

            var qtPN = "Questionstype";
            var qtPi = questionType.GetProperty(qtPN);

            var qtV = (String)qtPi.GetValue(doQuestionDTO);

            var scoreMethodName = "do" + qtV + "Question";
            var scoreMethod = scoreSvcType.GetMethod(scoreMethodName);

            if (scoreMethod == null)
            {
                throw new ArgumentException("Wherer method: " + scoreMethodName, nameof(scoreMethod));
            }

            var quesPropertyName = "QuestionID";
            var quesPropertyInfo = questionType.GetProperty(quesPropertyName);
            var quesID = (int)quesPropertyInfo.GetValue(doQuestionDTO);
            var question = await _dataContext.Questions.FirstOrDefaultAsync(e => e.Id == quesID);

            var score = await (Task<float>)scoreMethod.Invoke(scoreSvcs, new object[] { doQuestionDTO, question });

            servicesResponse.Message = "OK";

            return servicesResponse;
        }

        public async Task<QuizResponse> saveQuizRes(QuizResponse quizRes)
        {

            var quizExs = await _dataContext.QuizResponses.FirstOrDefaultAsync(e => e.QuestionId == quizRes.QuestionId && e.AccessId == quizRes.AccessId);

            if (quizExs == null)
            {
                _dataContext.QuizResponses.Add(quizRes);
            }
            else
            {

                _dataContext.QuizResponses.Update(quizRes);
            }

            await _dataContext.SaveChangesAsync();

            return quizRes;
        }

        public async Task<QuizResponse> saveMark<T>(int questionID, int quizAccessID, float mark, T Answer) where T : class
        {
            var quizRes = new QuizResponse();

            quizRes.AccessId = quizAccessID;
            quizRes.Mark = mark;
            quizRes.QuestionId = questionID;
            quizRes.Answer = JsonConvert.SerializeObject(Answer);

            return await saveQuizRes(quizRes);
        }

        public async Task<QuizResponse> saveMark<T>(int questionID, int quizAccessID, float mark, List<T> Answer) where T : class
        {
            var quizRes = new QuizResponse();

            quizRes.AccessId = quizAccessID;
            quizRes.Mark = mark;
            quizRes.QuestionId = questionID;
            quizRes.Answer = JsonConvert.SerializeObject(Answer);

            return await saveQuizRes(quizRes);
        }

        public async Task<float> scoreMatchQuestions(List<MatchSubQuestionResponseDTO> matchSubDtos, Question question)
        {
            var defaultMark = (float)question.DefaultMark;
            var markMatchSub = defaultMark / matchSubDtos.Count;

            matchSubDtos.ForEach(async matchSubDto =>
            {
                var matchSubCorrect = await _dataContext.MatchSubQuestions.FirstOrDefaultAsync(e => e.Id == matchSubDto.Id);

                if (!matchSubCorrect.AnswerText.Equals(matchSubDto.AnswerText))
                {
                    defaultMark -= markMatchSub;
                }
            });

            return defaultMark;
        }

        public async Task<float> doMatchQuestion(DoMatchingDTO doQuestionDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreMatchQuestions(doQuestionDTO.MatchSubs, question);

            await saveMark(doQuestionDTO.QuestionID, doQuestionDTO.QuizAccessID, mark, doQuestionDTO.MatchSubs);

            return mark;
        }

        public async Task<float> scoreMultiChoiceQuestions(List<DoMultipleAnswerDTO> doMultipleAnswers, Question question)
        {
            float sumFraction = 0;
            bool correctAll = true;

            doMultipleAnswers.ForEach(async doMultipleAnswer =>
            {
                var answerCorrect = await _dataContext.QuestionAnswers.FirstOrDefaultAsync(e => e.Id == doMultipleAnswer.AnswerId && e.QuestionId == question.Id);

                if (answerCorrect.Fraction == 0)
                {
                    correctAll = false;
                }
            });

            if (correctAll)
            {
                sumFraction = (float)question.DefaultMark;
            }

            return sumFraction;
        }

        public async Task<float> doMultiChoiceQuestion(DoMultipleDTO doQuestionDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreMultiChoiceQuestions(doQuestionDTO.Answers, question);

            await saveMark(doQuestionDTO.QuestionID, doQuestionDTO.QuizAccessID, mark, doQuestionDTO.Answers);

            return mark;
        }

        public async Task<float> scoreTrueFalseQuestion(DoTrueFalseAnswerDTO doTrueFalseAnswer, Question question)
        {
            float sumFraction = 0;

            var answerCorrect = await _dataContext.QuestionAnswers.FirstOrDefaultAsync(e => e.Id == doTrueFalseAnswer.AnswerId && e.QuestionId == question.Id);

            sumFraction += ((float)question.DefaultMark * answerCorrect.Fraction);

            return sumFraction;
        }

        public async Task<float> doTrueFalseQuestion(DoTrueFalseDTO doTrueFalseDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreTrueFalseQuestion(doTrueFalseDTO.Answers, question);

            await saveMark(doTrueFalseDTO.QuestionID, doTrueFalseDTO.QuizAccessID, mark, doTrueFalseDTO.Answers);

            return mark;
        }

        public async Task<float> scoreShortAnswerQuestion(DoShortAnswerDTO doShortAnswerDTO, Question question)
        {
            float sumFraction = 0;

            var answerCorrect = await _dataContext.QuestionAnswers.FirstOrDefaultAsync(e => e.Id == doShortAnswerDTO.AnswerId && e.QuestionId == question.Id && e.Content.Equals(doShortAnswerDTO.Content));

            if (answerCorrect != null)
            {
                sumFraction += ((float)question.DefaultMark * answerCorrect.Fraction);
            }

            return sumFraction;
        }

        public async Task<float> doShortAnswerQuestion(DoShortDTO doShortDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreShortAnswerQuestion(doShortDTO.Answers, question);

            await saveMark(doShortDTO.QuestionID, doShortDTO.QuizAccessID, mark, doShortDTO.Answers);

            return mark;
        }

        public async Task<float> scoreDragAndDropIntoTextQuestion(List<DoDragDropChoiceDTO> doDragDropChoiceDtos, Question question)
        {
            float sumFraction = 0;
            var defaultMark = (float)question.DefaultMark;
            var dragDropChoices = _dataContext.QuestionAnswers.Where(e => e.QuestionId == question.Id).ToList();
            var markMatchSub = defaultMark / dragDropChoices.Count;

            doDragDropChoiceDtos.ForEach(async doDragDropChoiceDto =>
            {
                var answerCorrect = dragDropChoices.ElementAtOrDefault(doDragDropChoiceDto.Position);

                if (answerCorrect != null && answerCorrect.Id.Equals(doDragDropChoiceDto.AnswerId))
                {
                    sumFraction += ((float)question.DefaultMark * answerCorrect.Fraction);
                }

            });

            return sumFraction;
        }

        public async Task<float> doDragAndDropIntoTextQuestion(DoDragDropTextDTO doDragDropTextDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreDragAndDropIntoTextQuestion(doDragDropTextDTO.Answers, question);

            await saveMark(doDragDropTextDTO.QuestionID, doDragDropTextDTO.QuizAccessID, mark, doDragDropTextDTO.Answers);

            return mark;
        }
    }
}
