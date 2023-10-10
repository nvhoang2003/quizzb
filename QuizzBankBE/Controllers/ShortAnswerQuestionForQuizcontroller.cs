using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Services.QuestionServices;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class ShortAnswerQuestionForQuizcontroller : ControllerBase
    {
        private readonly IShortAnswerQuestionServices _saQuestionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ShortAnswerQuestionForQuizcontroller(IShortAnswerQuestionServices saQuestionServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _saQuestionServices = saQuestionServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewQuesstion")]
        public async Task<ActionResult<ServiceResponse<ShortAnswerQuestionDTO>>> createNewQuestionBank(
               [FromBody] CreateShortAnswerQuestionDTO createQuestionDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUESTION").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            createQuestionDTO.AuthorId = userIdLogin;
            var response = await _saQuestionServices.CreateShortAnswerQuestion(createQuestionDTO);

            return Ok(response);
        }

        [HttpGet("GetQuestionBankById/{Id}")]
        public async Task<ActionResult<ServiceResponse<TrueFalseQuestionDTO>>> getQuestionByID(int Id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUESTION").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _saQuestionServices.GetShortAnswerQuestionById(Id);
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

        [HttpDelete("DeleteQuestionBank/{id}")]
        public async Task<ActionResult<ServiceResponse<TrueFalseQuestionDTO>>> deleteQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUESTION").Value;
            var deleteQuestion = await _saQuestionServices.GetShortAnswerQuestionById(id);

            if (!CheckPermission.Check(userIdLogin, permissionName) || userIdLogin != deleteQuestion.Data?.AuthorId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _saQuestionServices.DeleteShortAnswerQuestion(id);
            return Ok(response);
        }
    }
}
