using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.AuthServices;
using QuizzBankBE.Services.QuestionServices;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizzBankBE.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]

    // Lam Phan Quyen Sau khi Xu li hoan tat QuestionCategories
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
        public async Task<ActionResult<ServiceResponse<QuestionResponseDTO>>> createNewQuestionn(
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

        [HttpGet("getListQuestions")]
        public async Task<ActionResult<ServiceResponse<PageList<QuestionBankEntryResponseDTO>>>> getListQuestions(
        [FromQuery] OwnerParameter ownerParameters, int categoryId)
        {
            var response = await _questionServices.getListQuestion(ownerParameters, categoryId);
            var metadata = new
            {
                response.Data.TotalCount,
                response.Data.PageSize,
                response.Data.CurrentPage,
                response.Data.TotalPages,
                response.Data.HasNext,
                response.Data.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("getQuestionById/{id}")]
        public async Task<ActionResult<PageList<QuestionBankEntryResponseDTO>>> getQuestionById(int id)
        {
            var response = await _questionServices.getQuestionById(id);

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
