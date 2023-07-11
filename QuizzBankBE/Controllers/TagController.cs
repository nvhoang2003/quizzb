using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.TagServices;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class TagsController : ControllerBase
    {
        private readonly ITagServices _tagServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public TagsController(ITagServices tagServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _tagServices = tagServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }
        [HttpPost("CreateNewTag")]
        public async Task<ActionResult<ServiceResponse<TagResponseDTO>>> createNewTag(
        [FromBody] CreateTagDTO createTagDTO)
        {
            var response = await _tagServices.createNewTag(createTagDTO);
            return Ok(response);
        }
        [HttpGet("getListAllTag")]
        public async Task<ActionResult<ServiceResponse<PageList<TagDTO>>>> GetListTag(
        [FromQuery] OwnerParameter ownerParameters)
        {
            var response = await _tagServices.getAllTag(ownerParameters);
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
        [HttpPut("updateTag/{id}")]
        public async Task<ActionResult<ServiceResponse<TagDTO>>> updateTag(
        [FromBody] CreateTagDTO updateTagDTO, int id)
        {
            var response = await _tagServices.updateTag(updateTagDTO, id);
            return Ok(response);
        }
        [HttpDelete("deleteTag/{id}")]
        public async Task<ActionResult<ServiceResponse<TagDTO>>> deleteTag(int id)
        {
            var response = await _tagServices.deleteTag(id);
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
