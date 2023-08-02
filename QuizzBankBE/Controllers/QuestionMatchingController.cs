using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;
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
    public class QuestionMatchingController : ControllerBase
    {
        private readonly IMatchingQuestionBankServices _matchingQuestionBankServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuestionMatchingController(IMatchingQuestionBankServices matchingQuestionBankServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _matchingQuestionBankServices = matchingQuestionBankServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMatchingResponseDTO>>> createMatchingQuestion(
                [FromBody] CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createQuestionBankMatchingDTO.AuthorId = userIdLogin;
            var response = await _matchingQuestionBankServices.createMatchingQuestionBank(createQuestionBankMatchingDTO);

            return Ok(response);
        }

        [HttpGet("GetById/{questionBankID}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMatchingResponseDTO>>> getDetail(int questionBankID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _matchingQuestionBankServices.getMatchSubsQuestionBankById(questionBankID);
            
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

        [HttpPut("Update/{questionBankID}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> updateMatchingQuestion(
             [FromBody] CreateQuestionBankMatchingDTO updateQuestionDTO, int questionBankID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var updateQuestion = await _matchingQuestionBankServices.getMatchSubsQuestionBankById(questionBankID);

            if (updateQuestion.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = updateQuestion.StatusCode,
                    Title = updateQuestion.Message
                });
            }

            if (!CheckPermission.check(userIdLogin, permissionName) || updateQuestion.Data?.AuthorId != userIdLogin)
            {
                return new StatusCodeResult(403);
            }

            updateQuestionDTO.AuthorId = userIdLogin;
            var response = await _matchingQuestionBankServices.updateMatchSubsQuestionBank(updateQuestionDTO, questionBankID);
            return Ok(response);
        }

        [HttpDelete("Delete/{questionBankID}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> deleteMatchingQuestion(int questionBankID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var deleteQuestion = await _matchingQuestionBankServices.getMatchSubsQuestionBankById(questionBankID);

            if (deleteQuestion.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = deleteQuestion.StatusCode,
                    Title = deleteQuestion.Message
                });
            }

            if (!CheckPermission.check(userIdLogin, permissionName) || deleteQuestion.Data?.AuthorId != userIdLogin)
            {
                return new StatusCodeResult(403);
            }

            var response = await _matchingQuestionBankServices.deleteMatchSubsQuestionBank(questionBankID);
            return Ok(response);
        }
    }
}
