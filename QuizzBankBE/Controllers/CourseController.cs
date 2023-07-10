using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.CourseServices;
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

        [HttpGet("GetCourses")]
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

        [HttpGet("GetCoursesByUser")]
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

        [HttpGet("GetCourses/{courseID}")]
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

        [HttpPost("CreateCourse")]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> createCourse([FromBody] CreateCourseDTO createCourseDTO)
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

        [HttpPut("UpdateCourse/{courseID}")]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> updateCourse([FromBody] CreateCourseDTO updateCourseDTO, int courseID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var accessRoleResponse = await accessRole(courseID, userIdLogin);

            if (accessRoleResponse.Status == false)
            {
                return new StatusCodeResult(403);
            }

            var response = await _courseServices.updateCourse(updateCourseDTO, courseID);

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

        [HttpDelete("DeleteCourse/{courseID}")]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> deleteCourse(int courseID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var accessRoleResponse = await accessRole(courseID, userIdLogin);

            if (accessRoleResponse.Status == false)
            {
                return new StatusCodeResult(403);
            }

            var response = await _courseServices.deleteCourse(courseID);

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

        private async Task<ServiceResponse<CourseDTO>> accessRole(int courseID, int userID)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();
            var userInCourse = await _dataContext.UserCourses.FirstOrDefaultAsync(c => c.UserId == userID && c.CoursesId == courseID);

            if (UserCourseDTO.checkPowerfullUserCourseRole(userInCourse?.Role) == false)
            {
                serviceResponse.updateResponse(403, "Không có quyền!");

                return serviceResponse;
            }

            return serviceResponse;
        }
    }
}
