using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
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
    public class QuestionBankController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IQuestionBankValidate _questionBankValidate;
        private readonly IQuestionBankServices _questionBankServices;

        public QuestionBankController(IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration, IQuestionBankValidate questionBankValidate, IQuestionBankServices questionBankServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
            _questionBankValidate = questionBankValidate;
            _questionBankServices = questionBankServices;
        }

        [HttpPost("CreateNewQuesstion")]
        public async Task<ActionResult<ServiceResponse<CreateQuestionBankDTO>>> createNewQuestionBank(
               [FromBody] CreateQuestionBankDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createQuestionDTO.AuthorId = userIdLogin;
            var errors = await _questionBankValidate.CheckValidate(createQuestionDTO);
            if(errors.Data.Count() > 0)
            {
                return BadRequest(errors);
            }
            var response = await _questionBankServices.CreateQuestionBank(createQuestionDTO);

            return Ok(response);
        }

        [HttpGet("GetQuestionBankById/{Id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankResponseDTO>>> GetQuestionBankById(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionBankServices.GetQuestionBankById(Id);
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

        [HttpPut("UpdateQuesstionbank/{Id}")]
        public async Task<ActionResult<ServiceResponse<CreateQuestionBankDTO>>> createNewQuestionBank(
               [FromBody] CreateQuestionBankDTO updateQuestionDTO, int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var updateQuestion = await _questionBankServices.GetQuestionBankById(Id);
            
            if(updateQuestion.Data == null)
            {
                return  new StatusCodeResult(404);
            }

            if (!CheckPermission.Check(userIdLogin, permissionName) || (updateQuestion.Data?.AuthorId != userIdLogin && CheckPermission.IsAdmin(userIdLogin)))
            {
                return new StatusCodeResult(403);
            }
            updateQuestionDTO.AuthorId = updateQuestion.Data?.AuthorId;

            var errors = await _questionBankValidate.CheckValidate(updateQuestionDTO);
            if (errors.Data?.Count() > 0)
            {
                return BadRequest(errors);
            }

            var response = await _questionBankServices.UpdateQuestionBank(Id, updateQuestionDTO);

            return Ok(response);
        }

        [HttpDelete("DeleteQuesstionbank/{Id}")]
        public async Task<ActionResult<ServiceResponse<QuestionBankResponseDTO>>> DeleteQuestionBankBy(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var delQuestion = await _questionBankServices.GetQuestionBankById(Id);
            if (!CheckPermission.Check(userIdLogin, permissionName) || (delQuestion.Data?.AuthorId != userIdLogin && !CheckPermission.IsAdmin(userIdLogin)))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionBankServices.DeleteQuestionBank(Id);
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
