using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
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
    public class ScoreController : ControllerBase
    {
        private readonly IScoreServicesImpl _scoreServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ScoreController(IScoreServicesImpl scoreServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _scoreServices = scoreServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("{accessID}")]
        public async Task<ActionResult<ServiceResponse<float>>> GetScore(int accessID)
        {
            var response = await _scoreServices.getScore(accessID);

            return Ok(response);
        }

        [HttpPost("{quizID}/doMatchQuestion")]
        public async Task<ActionResult<ServiceResponse<float>>> DoMatchQuestion([FromBody] DoMatchingDTO doQuestionDTO, int quizID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var hasQuizAccess = await HaveQuizAccess(doQuestionDTO.QuizAccessID, userIdLogin, quizID);

            if (!hasQuizAccess)
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.doQuestion(doQuestionDTO);

            return Ok(response);
        }

        [HttpPost("{quizID}/doMultipeQuestion")]
        public async Task<ActionResult<ServiceResponse<float>>> DoMultipeQuestion([FromBody] DoMultipleDTO doQuestionDTO, int quizID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var hasQuizAccess = await HaveQuizAccess(doQuestionDTO.QuizAccessID, userIdLogin, quizID);

            if (!hasQuizAccess)
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.doQuestion(doQuestionDTO);

            return Ok(response);
        }

        [HttpPost("{quizID}/doTrueFalseQuestion")]
        public async Task<ActionResult<ServiceResponse<float>>> DoTrueFalseQuestion([FromBody] DoTrueFalseDTO doQuestionDTO, int quizID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var hasQuizAccess = await HaveQuizAccess(doQuestionDTO.QuizAccessID, userIdLogin, quizID);

            if (!hasQuizAccess)
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.doQuestion(doQuestionDTO);

            return Ok(response);
        }

        [HttpPost("{quizID}/doShortAnswerQuestion")]
        public async Task<ActionResult<ServiceResponse<float>>> DoShortAnswerQuestion([FromBody] DoShortDTO doQuestionDTO, int quizID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var hasQuizAccess = await HaveQuizAccess(doQuestionDTO.QuizAccessID, userIdLogin, quizID);

            if (!hasQuizAccess)
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.doQuestion(doQuestionDTO);

            return Ok(response);
        }

        [HttpPost("{quizID}/test/doMatchQuestion")]
        public async Task<ActionResult<ServiceResponse<float>>> TestMatchQuestion([FromBody] DoMatchingDTO doQuestionDTO, int quizID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:CHECK_QUIZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.doQuestion(doQuestionDTO);

            return Ok(response);
        }

        private async Task<bool> HaveQuizAccess(int quizAcessID, int userID, int quizID)
        {
            var quizAccess = await _dataContext.QuizAccesses.FirstOrDefaultAsync(e => e.Id == quizAcessID && e.UserId == userID && e.QuizId == quizID);

            if (quizAccess == null)
            {
                return false;
            }

            return true;
        }
    }
}
