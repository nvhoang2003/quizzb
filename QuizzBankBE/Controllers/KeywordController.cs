using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.Services.KeywordServices;
using QuizzBankBE.Services.QuestionServices;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class KeywordController : ControllerBase
    {
        private readonly IKeywordService _keywordServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public KeywordController(IKeywordService keywordServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _keywordServices = keywordServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }


        [HttpPost("CreateNewKeyword")]
        public async Task<ActionResult<ServiceResponse<KeywordDTO>>> createNewQuestionn(
        [FromBody] CreateKeywordDTO createKeywordDTO)
        {
            var response = await _keywordServices.CreateNewKeyword(createKeywordDTO);

            return Ok(response);
        }

        [HttpGet("getListAllKeyword")]
        public async Task<ActionResult<ServiceResponse<PageList<QuestionBankEntryResponseDTO>>>> getListQuestions(
        [FromQuery] OwnerParameter ownerParameters)
        {
            var response = await _keywordServices.getAllKeyword(ownerParameters);
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

        [HttpGet("getKeywordByCourseID/{id}")]
        public async Task<ActionResult<PageList<KeywordDTO>>> getKeywordByCourseID([FromQuery] OwnerParameter ownerParameters, int id)
        {
            var response = await _keywordServices.getListKeywordByCourseID(ownerParameters, id);
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

        [HttpPut("updateKeyword/{id}")]
        public async Task<ActionResult<ServiceResponse<KeywordDTO>>> updateKeyword(
        [FromBody] CreateKeywordDTO createKeywordDTO, int id)
        {
           
            var response = await _keywordServices.updateKeyword(createKeywordDTO, id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<KeywordDTO>>> deleteQuestion(int id)
        {
            //var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _keywordServices.deleteKeyword(id);
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
