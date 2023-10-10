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
using QuizzBankBE.Utility;
using System.Security.Claims;

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
        public async Task<ActionResult<ServiceResponse<RoleResponseDTO>>> createNewRole(
        [FromBody] CreateRoleDTO createRoleDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_ROLE").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _roleService.CreateNewRole(createRoleDTO);
            return Ok(response);
        }

        [HttpGet("GetAllRole")]
        public async Task<ActionResult<ServiceResponse<PageList<RoleDTO>>>> getAllRole(
           [FromQuery] OwnerParameter ownerParameters)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_ROLE").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var role = await _roleService.GetAllRole(ownerParameters);
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

        [HttpGet("GetRoleById/{id}")]
        public async Task<ActionResult<ServiceResponse<PageList<RoleDTO>>>> getRoleById(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_ROLE").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _roleService.GetRoleByID(id);
            return Ok(response);
        }

        [HttpPut("UpdateRole/{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDTO>>> updateRole(
        [FromBody] CreateRoleDTO updateRoleDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_ROLE").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _roleService.UpdateRole(updateRoleDTO, id);
            return Ok(response);
        }

        [HttpDelete("DeleteRole/{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDTO>>> deleteRole(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_ROLE").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _roleService.DeleteRole(id);
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
