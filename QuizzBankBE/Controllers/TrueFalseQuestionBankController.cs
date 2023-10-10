using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model;
using QuizzBankBE.Services.QuestionBankServices;
using QuizzBankBE.Utility;
using static QuizzBankBE.DTOs.QuestionBankDTOs.BaseQuestionBankDTO;
using System.Security.Claims;
using QuizzBankBE.DataAccessLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using QuizzBankBE.DTOs.QuestionBankDTOs;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class TrueFalseQuestionBankController : ControllerBase
    {
        private readonly ITrueFalseQuestionBankService _trueFalseQuestionBankService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public TrueFalseQuestionBankController(ITrueFalseQuestionBankService trueFalseQuestionBankService, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _trueFalseQuestionBankService = trueFalseQuestionBankService;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewTrueFalseQuestionBank")]
        public async Task<ActionResult<ServiceResponse<TrueFalseQuestionBankDTO>>> createNewTrueFalseQuestionBank(
              [FromBody] CreateTrueFalseQuestionDTO createTFQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createTFQuestionDTO.AuthorId = userIdLogin;
            var response = await _trueFalseQuestionBankService.CreateNewTrueFalseQuestionBank(createTFQuestionDTO);

            return Ok(response);
        }

        [HttpGet("GetTrueFalseQuestionBankById/{Id}")]
        public async Task<ActionResult<ServiceResponse<TrueFalseQuestionBankDTO>>> getTrueFalseQuestionBankById(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _trueFalseQuestionBankService.GetTrueFalseQuestionBankById(Id);
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


        [HttpPut("UpdateTrueFalseQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<TrueFalseQuestionBankService>>> updateTrueFalseQuestionBank(
               [FromBody] CreateTrueFalseQuestionDTO updateQuestionDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var updateQuestion = await _trueFalseQuestionBankService.GetTrueFalseQuestionBankById(id);

            if (!CheckPermission.Check(userIdLogin, permissionName) || updateQuestion.Data?.AuthorId != userIdLogin)
            {
                return new StatusCodeResult(403);
            }

            var response = await _trueFalseQuestionBankService.UpdateTrueFalseQuestionBank(updateQuestionDTO, id);
            return Ok(response);
        }

        [HttpDelete("DeleteTrueFalseQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<TrueFalseQuestionBankDTO>>> deleteTrueFalseQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var deleteQuestion = await _trueFalseQuestionBankService.GetTrueFalseQuestionBankById(id);

            if (!CheckPermission.Check(userIdLogin, permissionName) || userIdLogin != deleteQuestion.Data?.AuthorId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _trueFalseQuestionBankService.DeleteTrueFalseQuestionBank(id);
            return Ok(response);
        }
    }
}
