using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.Services.CategoryServices;
using QuizzBankBE.Services.RolePermissionServices;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Services.CourseServices;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionServices _rolePermissionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public RolePermissionController(IRolePermissionServices rolePermissionServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _rolePermissionServices = rolePermissionServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("GetDetail/{roleID}")]
        public async Task<ActionResult<ServiceResponse<RoleDetailPermissionsDTO>>> GetDetail(int roleID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_ROLE_PERMISSION").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _rolePermissionServices.GetDetailRolePermissions(roleID);
            
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

        [HttpPut("UpdatePermissions/{roleID}")]
        public async Task<ActionResult<ServiceResponse<RoleDetailPermissionsDTO>>> UpdatePermissions([FromBody] List<PermissionDTO> permissionDTOs, int roleID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_ROLE_PERMISSION").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _rolePermissionServices.UpdatePermissions(permissionDTOs ,roleID);

            return Ok(response);
        }
    }
}
