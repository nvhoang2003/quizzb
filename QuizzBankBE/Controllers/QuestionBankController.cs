using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Services.CourseServices;
using QuizzBankBE.Services.QuestionBankServices;
using QuizzBankBE.Utility;
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
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> createNewMultipeChoiceQuestionBank(
                [FromBody] CreateQuestionBankMultipeChoiceDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createQuestionDTO.AuthorId = userIdLogin;
            var response = await _multipeChoiceQuizBankServices.createNewMultipeQuestionBank(createQuestionDTO);

            return Ok(response);
        }

        [HttpGet("GetQuestionBankById/{Id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> getDetail(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _multipeChoiceQuizBankServices.getMultipeQuestionBankById(Id);
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

        [HttpPut("UpdateQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> updateMultipeChoiceQuestionBank(
               [FromBody] CreateQuestionBankMultipeChoiceDTO updateQuestionDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var updateQuestion = await _multipeChoiceQuizBankServices.getMultipeQuestionBankById(id);

            if (!CheckPermission.check(userIdLogin,permissionName) || updateQuestion.Data?.AuthorId != userIdLogin)
            {
                return new StatusCodeResult(403);
            }

            var response = await _multipeChoiceQuizBankServices.updateMultipeQuestionBank(updateQuestionDTO, id);
            return Ok(response);
        }

        [HttpDelete("DeleteQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> deleteMultipeChoiceQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var deleteQuestion = await _multipeChoiceQuizBankServices.getMultipeQuestionBankById(id);

            if (!CheckPermission.check(userIdLogin, permissionName) || userIdLogin != deleteQuestion.Data?.AuthorId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _multipeChoiceQuizBankServices.deleteMultipeQuestionBank(id);
            return Ok(response);
        }
    }
}
