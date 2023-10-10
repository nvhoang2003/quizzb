using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
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
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizAccessService.CreateQuizzAccess(createQuizDTO);

            return Ok(response);
        }


        [HttpPut("updateStausQuizzAccess/{id}")]
        public async Task<ActionResult<ServiceResponse<QuizAccessDTO>>> updateStatus(
              [FromBody] CreateQuizAccessDTO updateQuestionDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _quizAccessService.UpdateStatus(updateQuestionDTO, id);
            return Ok(response);
        }

    }
}
