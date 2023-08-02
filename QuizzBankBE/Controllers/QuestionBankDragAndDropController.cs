using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
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
    public class QuestionBankDragAndDropController : ControllerBase
    {
        private readonly IDragAndDropQuestionBank _dragAndDropQuestionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuestionBankDragAndDropController(IDragAndDropQuestionBank shortAnswerQuestionServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _dragAndDropQuestionServices = shortAnswerQuestionServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewQuesstion")]
        public async Task<ActionResult<ServiceResponse<QBankDragAndDropDTO>>> createNewQuestionBank(
                [FromBody] CreateQBankDragAndDropDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createQuestionDTO.AuthorId = userIdLogin;
            var response = await _dragAndDropQuestionServices.createDDQuestionBank(createQuestionDTO);

            return Ok(response);
        }

        [HttpGet("GetQuestionBankById/{Id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankShortAnswerDTO>>> getQuestionByID(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _dragAndDropQuestionServices.getDDQuestionBankById(Id);
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
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> updateQuestionBank(
           [FromBody] CreateQBankDragAndDropDTO updateQuestionDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var updateQuestion = await _dragAndDropQuestionServices.getDDQuestionBankById(id);

            if (!CheckPermission.check(userIdLogin, permissionName) || updateQuestion.Data?.AuthorId != userIdLogin)
            {
                return new StatusCodeResult(403);
            }

            var response = await _dragAndDropQuestionServices.updateDDQuestionBank(updateQuestionDTO, id);
            return Ok(response);
        }

        [HttpDelete("DeleteQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>>> deleteQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var deleteQuestion = await _dragAndDropQuestionServices.getDDQuestionBankById(id);

            if (!CheckPermission.check(userIdLogin, permissionName) || userIdLogin != deleteQuestion.Data?.AuthorId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _dragAndDropQuestionServices.deleteDDQuestionBank(id);
            return Ok(response);
        }
    }
}
