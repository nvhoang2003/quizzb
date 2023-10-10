using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.QuizService;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]

    public class QuizzController : ControllerBase
    {
        private readonly IQuizService _quizServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;


        public QuizzController(IQuizService quizServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _quizServices = quizServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("getListAllQuizz")]
        public async Task<ActionResult<ServiceResponse<PageList<QuizDTO>>>> getListQuizz(
        [FromQuery] OwnerParameter ownerParameters, string? name, DateTime? timeStart, DateTime? timeEnd, bool? isPublic, int? courseId)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizServices.getAllQuiz(ownerParameters, name, timeStart, timeEnd, isPublic, courseId);
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

        [HttpPost("CreateNewQuizz")]
        public async Task<ActionResult<ServiceResponse<QuizResponseDTO>>> createNewQizz(
        [FromBody] CreateQuizDTO createQuizDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizServices.createNewQuiz(createQuizDTO);

            return Ok(response);
        }

        [HttpPost("AddQuestion")]
        public async Task<ActionResult<ServiceResponse<QuizQuestionDTO>>> addQuizQuestion([FromBody] CreateQuizQuestionDTO createQuizQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizServices.addQuestionIntoQuiz(createQuizQuestionDTO);

            return Ok(response);
        }

        [HttpPut("updateQuiz/{id}")]
        public async Task<ActionResult<ServiceResponse<QuizDTO>>> updateQuiz(
        [FromBody] CreateQuizDTO updateQuizDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizServices.updateQuizz(updateQuizDTO, id);

            return Ok(response);
        }

        [HttpDelete("deleteQuiz/{id}")]
        public async Task<ActionResult<ServiceResponse<QuizDTO>>> deleteQuiz(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizServices.deleteQuizz(id);
            if (response.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = response.StatusCode,
                    Title = response.Message
                });
            }
            
            return Ok(response);
        }

        [HttpGet("getQuizById/{id}")]
        public async Task<ActionResult<QuizDetailResponseDTO>> getQuizById(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizServices.getQuizById(id);
            return Ok(response);
        }

        [HttpGet("getQuizForTest/{accessId}")]
        public async Task<ActionResult<QuizDetailResponseDTO>> getQuizForTest(int accessId)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:DO_QUIZZ").Value;

            var quizAccess = await _dataContext.QuizAccesses.Where(q => q.Id == accessId).FirstOrDefaultAsync();

            if (!CheckPermission.check(userIdLogin, permissionName) || userIdLogin != quizAccess.UserId)
            {
                return new StatusCodeResult(403);
            }

            string userName = await _dataContext.Users.Where(q => q.Id == userIdLogin).Select(q => q.FirstName + " " + q.LastName).FirstOrDefaultAsync();

            var response = await _quizServices.showQuizForTest((int)quizAccess.QuizId, userName);
            return Ok(response);
        }
    }
}
