using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.Services.UserServices;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using QuizzBankBE.Services.TagServices;
using Microsoft.EntityFrameworkCore;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public UserController(IUserServices userServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _userServices = userServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }
        [HttpGet("GetAllUser")]
        public async Task<ActionResult<ServiceResponse<PageList<UserDTO>>>> getAllUsers(
            [FromQuery] OwnerParameter ownerParameters)
        {

            var users = await _userServices.getAllUsers(ownerParameters);
            var metadata = new
            {
                users.Data.TotalCount,
                users.Data.PageSize,
                users.Data.CurrentPage,
                users.Data.TotalPages,
                users.Data.HasNext,
                users.Data.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(users);
        }

        [HttpPost("registerSingleUser")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> registerSingleUser(
        [FromBody] CreateUserDTO createUserDTO)
        {
            var response = await _userServices.createUsers(createUserDTO);
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


        [HttpGet("getUserById/{id}")]
        public async Task<ActionResult<UserDTO>> getUserById(int id)
        {
            var response = await _userServices.getUserByUserID(id);
            return Ok(response);
        }

        [HttpPut("updateUser/{id}")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> updateUser(
        [FromBody] UpdateUserDTO updateUserDTO, int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (!userIdLogin.Equals(id)) {
                return new StatusCodeResult(403);
            }

            var response = await _userServices.updateUser(updateUserDTO, id);
            return Ok(response);
        }
    }
}
