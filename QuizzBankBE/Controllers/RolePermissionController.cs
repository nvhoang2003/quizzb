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
            var response = await _rolePermissionServices.getDetailRolePermissions(roleID);

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
            var response = await _rolePermissionServices.updatePermissions(permissionDTOs ,roleID);

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
