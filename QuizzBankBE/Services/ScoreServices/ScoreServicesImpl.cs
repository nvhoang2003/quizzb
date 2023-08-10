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

        public async Task<ServiceResponse<DoQuizResponseDTO>> GetScore(int accessID)
        {
            var servicesResponse = new ServiceResponse<DoQuizResponseDTO>();
            var doQuizResponseDTO = new DoQuizResponseDTO();



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

        public async Task<float> scoreMatchQuestions(List<MatchSubQuestionResponseDTO> matchSubDtos, float defaultMark)
        {
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

            var mark = await scoreMatchQuestions(doQuestionDTO.MatchSubs, (float)question.DefaultMark);

            await saveMark(doQuestionDTO.QuestionID, doQuestionDTO.QuizAccessID, mark, doQuestionDTO.MatchSubs);

            return mark;
        }

        public async Task<float> scoreMultiChoiceQuestions(List<DoMultipleAnswerDTO> doMultipleAnswers, int questionID)
        {
            float sumFraction = 0;

            doMultipleAnswers.ForEach(async doMultipleAnswer =>
            {
                var answerCorrect = await _dataContext.QuestionAnswers.FirstOrDefaultAsync(e => e.Id == doMultipleAnswer.AnswerId && e.QuestionId == questionID);

                if (answerCorrect.Fraction != 0)
                {
                    sumFraction += answerCorrect.Fraction;
                }
            });

            return sumFraction;
        }

        public async Task<float> doMultiChoiceQuestion(DoMultipleDTO doQuestionDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreMultiChoiceQuestions(doQuestionDTO.Answers, question.Id);

            await saveMark(doQuestionDTO.QuestionID, doQuestionDTO.QuizAccessID, mark, doQuestionDTO.Answers);

            return mark;
        }

        public async Task<float> scoreTrueFalseQuestions(DoTrueFalseAnswerDTO doTrueFalseAnswer, int questionID)
        {
            float sumFraction = 0;

            var answerCorrect = await _dataContext.QuestionAnswers.FirstOrDefaultAsync(e => e.Id == doTrueFalseAnswer.AnswerId && e.QuestionId == questionID);

            sumFraction += answerCorrect.Fraction;

            return sumFraction;
        }

        public async Task<float> doTrueFalseQuestion(DoTrueFalseDTO doTrueFalseDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreTrueFalseQuestions(doTrueFalseDTO.Answers, question.Id);

            await saveMark(doTrueFalseDTO.QuestionID, doTrueFalseDTO.QuizAccessID, mark, doTrueFalseDTO.Answers);

            return mark;
        }

        public async Task<float> scoreShortAnswerQuestions(DoShortAnswerDTO doShortAnswer, int questionID)
        {
            float sumFraction = 0;

            var answerCorrect = await _dataContext.QuestionAnswers.FirstOrDefaultAsync(e => e.Id == doShortAnswer.AnswerId && e.QuestionId == questionID && e.Content.Equals(doShortAnswer.Content));

            if (answerCorrect != null)
            {
                sumFraction += answerCorrect.Fraction;
            }

            return sumFraction;
        }

        public async Task<float> doShortAnswerQuestion(DoShortDTO doShortDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var mark = await scoreShortAnswerQuestions(doShortDTO.Answers, question.Id);

            await saveMark(doShortDTO.QuestionID, doShortDTO.QuizAccessID, mark, doShortDTO.Answers);

            return mark;
        }
    }
}
