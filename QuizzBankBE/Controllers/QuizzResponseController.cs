using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.QuizzResponse;
using QuizzBankBE.Services.ScoreServices;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class QuizzResponseController : ControllerBase
    {
        private readonly IQuizResponseServices _quizResponseServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuizzResponseController(IQuizResponseServices quizResponseServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _quizResponseServices = quizResponseServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("listResponseForPeopleDoQuiz")]
        public async Task<ActionResult<PageList<AllQuizzResponseDTO>>> listResponseForPeopleDoQuiz([FromQuery] OwnerParameter ownerParameter, int? quizId, int? courseId, DateTime timeStart, DateTime timeEnd)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            
            var response = await _quizResponseServices.GetListResponseForDoQuiz(ownerParameter, userIdLogin, quizId, courseId, timeStart, timeEnd);
            var metadata = new
            {
                response.Data.TotalCount,
                response.Data.PageSize,
                response.Data.CurrentPage,
                response.Data.TotalPages,
                response.Data.HasNext,
                response.Data.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        [HttpGet("listResponseForPeopleWriteQuiz")]
        public async Task<ActionResult<PageList<AllQuizzResponseDTO>>> listResponseForPeopleWriteQuiz([FromQuery] OwnerParameter ownerParameter, int? quizId, int? courseId, string? name)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZZ_RESPONSE").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizResponseServices.GetListResponseForWriteQuiz(ownerParameter, quizId, courseId, name);
            var metadata = new
            {
                response.Data.TotalCount,
                response.Data.PageSize,
                response.Data.CurrentPage,
                response.Data.TotalPages,
                response.Data.HasNext,
                response.Data.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        [HttpGet("getQuizzResponse/{id}")]
        public async Task<ActionResult<AllQuizzResponseDTO>> GetQuizzResponse(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZZ_RESPONSE").Value;
            var accessQuizz = _dataContext.QuizAccesses.Where(q => q.Id == id).FirstOrDefaultAsync().Result;
            if (accessQuizz == null)
            {
                return new StatusCodeResult(404);
            }

            if (!CheckPermission.Check(userIdLogin, permissionName) && userIdLogin != accessQuizz.UserId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizResponseServices.GetResponseDetail(id);
            return Ok(response);
        }
    }
}
