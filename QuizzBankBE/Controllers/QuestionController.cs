using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Services.AuthServices;
using QuizzBankBE.Services.QuestionServices;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionServices _questionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuestionController(IQuestionServices questionServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _questionServices = questionServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewQuestion")]
        public async Task<ActionResult<ServiceResponse<QuestionResponseDTO>>> logiCreateNewQuestionn(
        [FromBody] CreateQuestionDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (userIdLogin == null)
            {
                return new StatusCodeResult(401);
            }

            createQuestionDTO.SetUserMutation(userIdLogin, userIdLogin);
            
            var response = await _questionServices.createNewQuestion(createQuestionDTO);
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
