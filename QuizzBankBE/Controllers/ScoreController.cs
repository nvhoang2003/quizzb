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
        private readonly IValidateScore _validateScoreServices;

        public ScoreController(IScoreServicesImpl scoreServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration, IValidateScore validateScoreServices)
        {
            _scoreServices = scoreServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
            _validateScoreServices = validateScoreServices;
        }

        [HttpGet("{accessID}")]
        public async Task<ActionResult<ServiceResponse<float>>> GetScore(int accessID)
        {
            var response = await _scoreServices.GetScore(accessID);

            return Ok(response);
        }      

        [HttpPost("SubmitQuizz")]
        public async Task<ActionResult<ServiceResponse<bool>>> SubmitTheQuiz<T>(
            [FromBody] int accessID, List<T> ListQuestionSubmit) where T : DoQuestionDTO
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var permissionCheckQuiz = _configuration.GetSection("Permission:CHECK_QUIZ").Value;
            var permissionDoQuiz = _configuration.GetSection("Permission:DO_QUIZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionCheckQuiz) && !CheckPermission.check(userIdLogin, permissionDoQuiz))
            {
                return new StatusCodeResult(403);
            }

            Dictionary<string, List<string>> errors = await _validateScoreServices.checkAccessId(accessID, userIdLogin);
            if(errors.Count() != 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }

            return Ok(true);
        }

      //  [HttpPost("CreateNewQuizz")]
      //  public async Task<ActionResult<ServiceResponse<QuizResponseDTO>>> createNewQizz(
      //[FromBody] CreateQuizDTO createQuizDTO)
      //  {
      //      var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
      //      var permissionName = _configuration.GetSection("Permission:WRITE_QUIZZ").Value;

      //      if (!CheckPermission.check(userIdLogin, permissionName))
      //      {
      //          return new StatusCodeResult(403);
      //      }

      //      var response = await _quizServices.createNewQuiz(createQuizDTO);

      //      return Ok(response);
      //  }
    }
}
