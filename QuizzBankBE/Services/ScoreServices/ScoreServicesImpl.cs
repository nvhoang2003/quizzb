using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using System.Reflection;

namespace QuizzBankBE.Services.ScoreServices
{
    public class ScoreServicesImpl : IScoreServicesImpl
    {
        public DataContext _dataContext;
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

        public async Task<ServiceResponse<float>> doQuestion<T>(T doQuestionDTO) where T : DoQuestionDTO
        {
            var servicesResponse = new ServiceResponse<float>();
            var scoreSvcType = typeof(ScoreServicesImpl);

            var questionType = doQuestionDTO.GetType();

            var qtPN = "Questionstype";
            var qtPi = questionType.GetProperty(qtPN);

            var qtV = (String)qtPi.GetValue(doQuestionDTO);

            var scoreMethodName = "do" + qtV + "Question";
            var scoreMethod = scoreSvcType.GetMethod(scoreMethodName);

            if (scoreMethod == null)
            {
                throw new ArgumentException("Wherer method: " + scoreMethodName , nameof(scoreMethod));
            }

            var quesPropertyName = "QuestionID";
            var quesPropertyInfo = questionType.GetProperty(quesPropertyName);
            var quesID = (int)quesPropertyInfo.GetValue(doQuestionDTO);
            var question = await _dataContext.Questions.FirstOrDefaultAsync(e => e.Id == quesID);

            var score = (float)scoreMethod.Invoke(null, new object[] { doQuestionDTO, question });

            servicesResponse.Message = "OK";

            return servicesResponse;
        }

        public async Task<QuizResponse> saveMark<T>(int questionID, int quizAccessID, float mark, T Answer)
        {
            var quizRes = new QuizResponse();

            quizRes.AccessId = quizAccessID;
            quizRes.Mark = mark;
            quizRes.QuestionId = questionID;
            quizRes.Answer = JsonConvert.SerializeObject(Answer);

            return await saveQuizRes(quizRes);
        }

        public async Task<QuizResponse> saveMark<T>(int questionID, int quizAccessID, float mark, List<T> Answer)
        {
            var quizRes = new QuizResponse();

            quizRes.AccessId = quizAccessID;
            quizRes.Mark = mark;
            quizRes.QuestionId = questionID;
            quizRes.Answer = JsonConvert.SerializeObject(Answer);

            return await saveQuizRes(quizRes);
        }

        public async Task<QuizResponse> saveQuizRes (QuizResponse quizRes)
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

        public async Task<float> doMatchQuestion(DoMatchingDTO doQuestionDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();
            float mark;

            var isCorrect = await scoreMatchQuestion(doQuestionDTO.MatchSubs);

            if (!isCorrect)
            {
                mark = 0;
            }

            mark = (float) question.DefaultMark;

            await saveMark<MatchSubQuestionBankDTO>(doQuestionDTO.QuestionID, doQuestionDTO.QuizAccessID, mark, doQuestionDTO.MatchSubs);

            return mark;
        }

        public async Task<bool> scoreMatchQuestion(List<MatchSubQuestionBankDTO> matchSubDtos)
        {
            foreach (var matchSubDto in matchSubDtos)
            {
                var matchSubCorrect = await _dataContext.MatchSubQuestionBanks.FirstOrDefaultAsync(e => e.Id == matchSubDto.Id);

                if (!matchSubCorrect.AnswerText.Equals(matchSubDto.AnswerText))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
