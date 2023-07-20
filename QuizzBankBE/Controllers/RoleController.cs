using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.Services.TagServices;
using QuizzBankBE.Services.RoleServices;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public RoleController(IRoleServices roleServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _roleService = roleServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewRole")]
        public async Task<ActionResult<ServiceResponse<RoleResponseDTO>>> createNewTag(
        [FromBody] CreateRoleDTO createRoleDTO)
        {
            var response = await _roleService.createNewRole(createRoleDTO);
            return Ok(response);
        }

        [HttpGet("GetAllRole")]
        public async Task<ActionResult<ServiceResponse<PageList<RoleDTO>>>> getAllRole(
           [FromQuery] OwnerParameter ownerParameters)
        {
            var role = await _roleService.getAllRole(ownerParameters);
            var metadata = new
            {
                role.Data.TotalCount,
                role.Data.PageSize,
                role.Data.CurrentPage,
                role.Data.TotalPages,
                role.Data.HasNext,
                role.Data.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(role);
        }

        [HttpPut("updateRole/{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDTO>>> updateRole(
        [FromBody] CreateRoleDTO updateRoleDTO, int id)
        {
            var response = await _roleService.updateRole(updateRoleDTO, id);
            return Ok(response);
        }

        [HttpDelete("deleteRole/{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDTO>>> deleteRole(int id)
        {
            var response = await _roleService.deleteRole(id);
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
