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
using QuizzBankBE.Utility;
using System.Security.Claims;
using static QuizzBankBE.DTOs.UserCourseDTO;

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
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_COURSE").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var courseResponse = await _courseServices.getAllCourse(ownerParameters);

            var courseResponsePagedList = SettingsPagination(courseResponse);

            return Ok(courseResponse);
        }

        [HttpGet("GetCoursesByUser")]
        public async Task<ActionResult<ServiceResponse<PageList<CourseDTO>>>> getAllCourseByUser(
            [FromQuery] OwnerParameter ownerParameters)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_COURSE").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var courseResponse = await _courseServices.getAllCourseByUserID(ownerParameters, userIdLogin);

            var courseResponsePagedList = SettingsPagination(courseResponse);

            return Ok(courseResponsePagedList);
        }

        [HttpGet("GetCourses/{courseID}")]
        public async Task<ActionResult<ServiceResponse<Course>>> getCourseByCourseID(int courseID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_COURSE").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

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
            var permissionName = _configuration.GetSection("Permission:READ_COURSE").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
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

        [HttpPut("UpdateCourse/{courseID}")]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> updateCourse([FromBody] CreateCourseDTO updateCourseDTO, int courseID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_COURSE").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
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
            var permissionName = _configuration.GetSection("Permission:READ_COURSE").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var checkCourseResponse = await _courseServices.getCourseByCourseID(courseID);

            if (checkCourseResponse.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = checkCourseResponse.StatusCode,
                    Title = checkCourseResponse.Message
                });
            }

            var response = await _courseServices.deleteCourse(courseID, checkCourseResponse.Data);

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

        private ServiceResponse<PageList<CourseDTO>> SettingsPagination (ServiceResponse<PageList<CourseDTO>> courseResponsePagedList)
        {
            var metadata = new
            {
                courseResponsePagedList.Data.TotalCount,
                courseResponsePagedList.Data.PageSize,
                courseResponsePagedList.Data.CurrentPage,
                courseResponsePagedList.Data.TotalPages,
                courseResponsePagedList.Data.HasNext,
                courseResponsePagedList.Data.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return courseResponsePagedList;
        }
    }
}
