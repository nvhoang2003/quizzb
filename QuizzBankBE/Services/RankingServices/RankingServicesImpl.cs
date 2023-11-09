using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Services.RankingServices
{
    public class RankingServicesImpl : IRankingServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RankingServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<RankingDTO>> GetRanking(OwnerParameter ownerParameters, int quizId)
        {
            var servicesResponse = new ServiceResponse<RankingDTO>();

            var isPermiss = await CheckReadRankingPermission(quizId);

            var userIdLogin = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            
            if (isPermiss == false)
            {
                servicesResponse.Status = false;
                servicesResponse.updateResponse(403, "Bạn không có quyền!");
                return servicesResponse;
            }

            var quiz = await _dataContext.Quizzes.Include(q => q.Course).Where(q => q.Id == quizId).FirstOrDefaultAsync();

            if(quiz == null)
            {
                servicesResponse.Status = false;
                servicesResponse.updateResponse(404, "Đề Không Tồn Tại!");
                return servicesResponse;
            }

            string sql = @"select u.ID as userId ,concat(u.firstName , ' ' , u.lastName) as fullName, sum(qr.mark) as totalPoint, TIMESTAMPDIFF(SECOND ,qa.timeStartQuiz, qa.timeEndQuiz) as totalTime from quizzb.quiz_accesses as qa 
	                left join quizzb.users as u on qa.userId = u.ID
                    inner join quizzb.quiz_responses as qr on qa.ID = qr.accessId
                    where qa.quizId = " + quizId + @" and qa.isDeleted != 1
                    group by u.ID
                    ORDER BY totalPoint DESC, totalTime ASC;";

            var listRanking = await _dataContext.Set<Ranking>().FromSqlRaw(sql).ToListAsync();

            var rankingResponse = new RankingDTO();

            rankingResponse.CourseId = quiz?.Course?.Id;
            rankingResponse.QuizName = quiz.Name;
            rankingResponse.YourRank = null;

            foreach(var item in listRanking)
            {
                var oneRanking = new OneDetailRankingDTO();
               
                oneRanking.Rank = listRanking.IndexOf(item) + 1;
                oneRanking.Score = item.TotalPoint;
                oneRanking.StudentName = item.FullName;
                oneRanking.TimeDoQuiz = $"{item.TotalTime / 60} phút {item.TotalTime % 60} giây";

                if (userIdLogin == item.UserId)
                {
                    rankingResponse.YourRank = oneRanking;
                }

                rankingResponse.ListRanking.Add(oneRanking);
            }

            servicesResponse.Data = rankingResponse;

            return servicesResponse;
        }

        public async Task<Boolean> CheckReadRankingPermission(int quizzId)
        {
            var userIdLogin = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = await _dataContext.Users.Include(q => q.Role).Include(u => u.QuizAccessUsers).Where(q => q.Id == userIdLogin).FirstOrDefaultAsync();

            if(user.Role.Name == "student")
            {
                if(!user.QuizAccessUsers.Select(q => q.QuizId).ToList().Contains(quizzId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                var permissionName = _configuration.GetSection("Permission:READ_QUIZZ_RESPONSE").Value;

                if (!CheckPermission.Check(userIdLogin, permissionName))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
