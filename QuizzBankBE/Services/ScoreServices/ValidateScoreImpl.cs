using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.ScoreServices
{
    public class ValidateScoreImpl : IValidateScore
    {
        public static DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public ValidateScoreImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public async Task<Dictionary<string, List<string>>> checkAccessId(int accessId, int userLoginId)
        {
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

            var accessDb = await _dataContext.QuizAccesses.Where(q => q.Id == accessId && q.Status == "Doing").FirstOrDefaultAsync();
            if(accessDb == null)
            {
                errors.Add("AccessId", new List<string> { "Không Tồn Tại Bài Thi." });
            }

            if(accessDb.UserId != userLoginId)
            {
                errors.Add("AccessId", new List<string> { "Bài thi này không phải của bạn." });
            }

            return errors;
        }

        public async Task<Dictionary<string, List<string>>> checkListQuestion<T>(List<T> listQuestions) where T: DoQuestionDTO
        {
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

            foreach(var question in listQuestions)
            {
                //if()
            }

            return errors;
        }
    }
}
