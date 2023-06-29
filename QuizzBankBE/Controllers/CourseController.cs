using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
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

        [HttpGet("GetAllCourse")]
        public async Task<ActionResult<ServiceResponse<PageList<CourseDTO>>>> getAllCourse(
            [FromQuery] OwnerParameter ownerParameters)
        {
            var course = await _courseServices.getAllCourse(ownerParameters);
            var metadata = new
            {
                course.Data.TotalCount,
                course.Data.PageSize,
                course.Data.CurrentPage,
                course.Data.TotalPages,
                course.Data.HasNext,
                course.Data.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(course);
        }

        [HttpGet("GetAllCourseByUser")]
        public async Task<ActionResult<ServiceResponse<PageList<CourseDTO>>>> getAllCourseByUser(
            [FromQuery] OwnerParameter ownerParameters)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var course = await _courseServices.getAllCourseByUserID(ownerParameters, userIdLogin);
            var metadata = new
            {
                course.Data.TotalCount,
                course.Data.PageSize,
                course.Data.CurrentPage,
                course.Data.TotalPages,
                course.Data.HasNext,
                course.Data.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(course);
        }

        [HttpGet()]
        public async Task<ActionResult<ServiceResponse<Course>>> getCourseByCourseID(int courseID)
        {
            var response = await _courseServices.getCourseByCourseID(courseID);

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

        [HttpPost()]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> createCourse([FromBody] BaseCourseDTO createCourseDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

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

        [HttpPut()]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> updateCourse([FromBody] BaseCourseDTO updateCourseDTO, int courseID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _courseServices.updateCourse(updateCourseDTO, courseID, userIdLogin);

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

        [HttpDelete()]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> deleteCourse(int courseID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _courseServices.deleteCourse(courseID, userIdLogin);

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
