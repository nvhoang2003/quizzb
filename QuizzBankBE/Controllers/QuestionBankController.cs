using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Services.QuestionBankServices;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class QuestionBankController : ControllerBase
    {
        private readonly IMultipeChoiceQuizBankServices _multipeChoiceQuizBankServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuestionBankController(IMultipeChoiceQuizBankServices multipeChoiceQuizBankServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _multipeChoiceQuizBankServices = multipeChoiceQuizBankServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewQuestion")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> createNewQuestionn(
                [FromBody] CreateQuestionBankMultipeChoiceDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (userIdLogin == null)
            {
                return new StatusCodeResult(401);
            }

            createQuestionDTO.AuthorId = userIdLogin;

            //createQuestionDTO.SetUserMutation(userIdLogin, userIdLogin);
            var response = await _multipeChoiceQuizBankServices.createNewMultipeQuestionBank(createQuestionDTO);

            return Ok(response);
        }

    }
}
