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

    public class QuizAccessController : ControllerBase
    {
        private readonly IQuizzAccessService _quizAccessService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;


        public QuizAccessController(IQuizzAccessService quizzAccess, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _quizAccessService = quizzAccess;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }


        [HttpPost("CreateNewQuizzAccess")]
        public async Task<ActionResult<ServiceResponse<QuizAccessDTO>>> createNewQizzAccess(
        [FromBody] CreateQuizAccessDTO createQuizDTO)
        {
            var response = await _quizAccessService.CreateQuizzAccess(createQuizDTO);

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


        [HttpPut("updateStausQuizzAccess/{id}")]
        public async Task<ActionResult<ServiceResponse<QuizAccessDTO>>> updateStatus(
              [FromBody] CreateQuizAccessDTO updateQuestionDTO, int id)
        {
            var response = await _quizAccessService.UpdateStatus(updateQuestionDTO, id);

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

        [HttpGet("GetListQuizzAccess")]
        public async Task<ActionResult<ServiceResponse<PageList<QuizAccessDTO>>>> getListQizzAccess(
       [FromQuery] OwnerParameter ownerParameters, int? courseId, string? studentName, string? status, bool? isPublic)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_LIST_STUDENT_DO_QUIZZ").Value;
            var user = await _dataContext.Users.Where(q => q.Id == userIdLogin).FirstAsync();

            if (!CheckPermission.Check(userIdLogin, permissionName) && userIdLogin != 2)
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizAccessService.GetListQuizzAccess(ownerParameters, courseId, studentName, status, isPublic);
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

        [HttpDelete("deleteQuizAccess/{id}")]
        public async Task<ActionResult<QuizAccessDTO>> deleteQuizAccess(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_LIST_STUDENT_DO_QUIZZ").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizAccessService.DeleteQuizAccess(id);
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
    }
}
