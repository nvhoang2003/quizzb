using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.QuizService;
using QuizzBankBE.Services.ScoreServices;
using QuizzBankBE.Utility;
using System.Dynamic;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var response = await _scoreServices.GetScore(accessID);

            return Ok(response);
        }      

        [HttpPost("SubmitQuizz")]
        public async Task<ActionResult<ServiceResponse<bool>>> SubmitTheQuiz(
            [FromBody] List<NewQuizResponse> ListQuestionSubmit, int accessID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:DO_QUIZZ").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            _scoreServices.doQuestion(ListQuestionSubmit);
            return Ok();
        }
    }
}
