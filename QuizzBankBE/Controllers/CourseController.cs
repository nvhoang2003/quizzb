using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.Model;
using QuizzBankBE.Services.CourseServices;
using QuizzBankBE.Services.QuestionServices;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public CourseController(ICourseServices courseServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _courseServices = courseServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateCourse")]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> createCourse([FromBody] BaseCourseDTO createCourseDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (userIdLogin == null)
            {
                return new StatusCodeResult(401);
            }

            var response = await _courseServices.createCourse(createCourseDTO, userIdLogin);

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
