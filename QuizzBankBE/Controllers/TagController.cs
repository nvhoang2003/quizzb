using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.CategoryServices;
using QuizzBankBE.Services.TagServices;
using QuizzBankBE.Utility;
using System.Security.Claims;

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
        public static IMapper _mapper;

        public TagsController(ITagServices tagServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration, IMapper mapper)
        {
            _tagServices = tagServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("CreateNewTag")]
        public async Task<ActionResult<ServiceResponse<TagResponseDTO>>> createNewTag(
        [FromBody] CreateBaseTagDTO createTagDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_TAG").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            string nameDTO = "CreateTagDTO";
            Type dtoType = Type.GetType(nameDTO);

            CreateTagDTO tag = _mapper.Map<CreateTagDTO>(createTagDTO);
            //object tag = _mapper.Map(createTagDTO, createTagDTO.GetType(), dtoType);
            TryValidateModel(tag);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var response = await _tagServices.CreateNewTag(tag);
            //return Ok(response);
            return Ok();
        }

        [HttpGet("getListAllTagByCategoryID")]
        public async Task<ActionResult<ServiceResponse<PageList<TagDTO>>>> GetListTagByCategoryID(
        [FromQuery] OwnerParameter ownerParameters, int categoryID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_TAG").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _tagServices.GetAllTagByCategoryID(ownerParameters,categoryID);
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

        [HttpGet("getTagById/{id}")]
        public async Task<ActionResult<TagDTO>> getTagById(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_TAG").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _tagServices.GetTagByID(id);
            return Ok(response);
        }

        [HttpPut("UpdateTag/{id}")]
        public async Task<ActionResult<ServiceResponse<TagDTO>>> updateTag(
        [FromBody] CreateTagDTO updateTagDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_TAG").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _tagServices.UpdateTag(updateTagDTO, id);
            return Ok(response);
        }

        [HttpDelete("DeleteTag/{id}")]
        public async Task<ActionResult<ServiceResponse<TagDTO>>> deleteTag(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_TAG").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _tagServices.DeleteTag(id);
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
