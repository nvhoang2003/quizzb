using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
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

        public async Task<ServiceResponse<float>> doQuestion<T>(T doQuestionDTO)
        {
            var servicesResponse = new ServiceResponse<float>();
            var scoreSvcType = typeof(ScoreServicesImpl);

            if (doQuestionDTO == null)
            {
                servicesResponse.updateResponse(500, "Where params ?");
                return servicesResponse;
            }

            var questionType = doQuestionDTO.GetType();

            if (!(doQuestionDTO is DoQuestionDTO))
            {
                servicesResponse.updateResponse(500, questionType.Name + " is not correct type !");
                return servicesResponse;
            }

            var qtPN = "Questionstype";
            var qtPi = questionType.GetProperty(qtPN);

            if (qtPi == null)
            {
                servicesResponse.updateResponse(500, "Wherer property: " + qtPN);
                return servicesResponse;
            }

            var qtV = (String)qtPi.GetValue(doQuestionDTO);

            var scoreMethodName = "do" + qtV + "Question";
            var scoreMethod = scoreSvcType.GetMethod(scoreMethodName);

            if (scoreMethod == null)
            {
                servicesResponse.updateResponse(500, "Wherer method: " + scoreMethodName);
                return servicesResponse;
            }

            var quesPName = "QuestionID";
            var quesPi = questionType.GetProperty(quesPName);
            var quesID = (int)quesPi.GetValue(doQuestionDTO);
            var question = await _dataContext.Questions.FirstOrDefaultAsync(e => e.Id == quesID);

            var score = (float)scoreMethod.Invoke(null, new object[] { doQuestionDTO, question });

            var qAPName = "QuizAccessID";
            var qAPi = questionType.GetProperty(qAPName);
            var qAV = (int)qAPi.GetValue(doQuestionDTO);

            servicesResponse.Message = "OK";

            return servicesResponse; 
        }

        public async void saveQuizAccess (int questionID, int quizAccessID, float mark)
        {

        }

        public async Task<float> doMatchQuestion(DoMatchingDTO doQuestionDTO, Question question)
        {
            var servicesResponse = new ServiceResponse<float>();

            var isCorrect = await scoreMatchQuestion(doQuestionDTO.MatchSubs);

            if (!isCorrect)
            {
                return 0;
            }

            return question.DefaultMark;
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
