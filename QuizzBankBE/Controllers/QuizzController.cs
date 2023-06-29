using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.KeywordServices;
using QuizzBankBE.Services.QuestionServices;
using QuizzBankBE.Services.QuizService;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]


    public class QuizzController : ControllerBase
    {
        private readonly IQuizService _quizServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;


        public QuizzController(IQuizService quizServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _quizServices = quizServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }


        [HttpPost("CreateNewQuizz")]
        public async Task<ActionResult<ServiceResponse<QuizResponseDTO>>> createNewQuestionn(
        [FromBody] CreateQuizDTO createQuizDTO)
        {
            var response = await _quizServices.createNewQuiz(createQuizDTO);

            return Ok(response);
        }


        [HttpGet("getListAllQuizz")]
        public async Task<ActionResult<ServiceResponse<PageList<QuizDTO>>>> getListQuizz(
        [FromQuery] OwnerParameter ownerParameters)
        {
            var response = await _quizServices.getAllQuiz(ownerParameters);
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


        [HttpPut("updateQuiz/{id}")]
        public async Task<ActionResult<ServiceResponse<QuizDTO>>> updateQuiz(
        [FromBody] UpdateQuizDTO updateQuizDTO, int id)
        {

            var response = await _quizServices.updateQuizz(updateQuizDTO, id);

            return Ok(response);
        }

        [HttpDelete("deleteQuiz/{id}")]
        public async Task<ActionResult<ServiceResponse<QuizDTO>>> deleteQuiz(int id)
        {

            var response = await _quizServices.deleteQuizz(id);
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
