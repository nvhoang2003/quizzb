using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
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
    public class NumericalQuestionController : ControllerBase
    {
        private readonly INumericalQuestionService _numericalQuestionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public NumericalQuestionController(INumericalQuestionService numericalQuestionServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _numericalQuestionService = numericalQuestionServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewNumericalQuesstion")]
        public async Task<ActionResult<ServiceResponse<NumericalQuestionDTO>>> createNewNumericalQuestionBank(
              [FromBody] CreateNumericalQuestionDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createQuestionDTO.AuthorId = userIdLogin;
            var response = await _numericalQuestionService.createNumericalQuestionBank(createQuestionDTO);

            return Ok(response);
        }


        [HttpGet("getNumericalQuestionBankById/{Id}")]
        public async Task<ActionResult<ServiceResponse<NumericalQuestionDTO>>> getNumericalQuestionBankById(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _numericalQuestionService.getNumericalQuestionBankById(Id);
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

        [HttpPut("updateNumericalQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<NumericalQuestionDTO>>> updateTrueFalseQuestionBank(
              [FromBody] CreateNumericalQuestionDTO updateQuestionDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var updateQuestion = await _numericalQuestionService.getNumericalQuestionBankById(id);

            if (!CheckPermission.check(userIdLogin, permissionName) || updateQuestion.Data?.AuthorId != userIdLogin)
            {
                return new StatusCodeResult(403);
            }

            var response = await _numericalQuestionService.updateNumericalQuestionBank(updateQuestionDTO, id);
            return Ok(response);
        }

        [HttpDelete("deleteNumericalQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<NumericalQuestionDTO>>> deleteNumericalQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;
            var deleteQuestion = await _numericalQuestionService.getNumericalQuestionBankById(id);

            if (!CheckPermission.check(userIdLogin, permissionName) || userIdLogin != deleteQuestion.Data?.AuthorId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _numericalQuestionService.deleteNumericalQuestionBank(id);
            return Ok(response);
        }
    }
}
