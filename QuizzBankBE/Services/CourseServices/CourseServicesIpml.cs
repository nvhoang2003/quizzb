using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.Data;
using AutoMapper;
using QuizzBankBE.JWT;
using Microsoft.EntityFrameworkCore;
using static QuizzBankBE.DTOs.UserCourseDTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace QuizzBankBE.Services.CourseServices
{
    public class CourseServicesIpml : ICourseServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<CourseDTO>> CreateCourse(CreateCourseDTO createCourseDto, int userIdLogin)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();

            var courseSaved = _mapper.Map<Course>(createCourseDto);

            _dataContext.Courses.Add(courseSaved);
            await _dataContext.SaveChangesAsync();

            var userCourseSaved = await CreateUserCourse(userIdLogin, courseSaved.Id);

            serviceResponse.Message = "Tạo thành công !";
            serviceResponse.Data = _mapper.Map<CourseDTO>(courseSaved);

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<CourseDTO>>> GetAllCourse(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<CourseDTO>>();
            var courseDTOs = new List<CourseDTO>();
            var dbCourse = await _dataContext.Courses.ToListAsync();

            courseDTOs = dbCourse.Select(u => _mapper.Map<CourseDTO>(u)).ToList();

            serviceResponse.Data = PageList<CourseDTO>.ToPageList(
            courseDTOs.AsEnumerable<CourseDTO>()/*.OrderBy(on => on.Courseid)*/,
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<CourseDTO>>> GetAllCourseByUserID(OwnerParameter ownerParameters, int userID)
        {
            var serviceResponse = new ServiceResponse<PageList<CourseDTO>>();
            var courseDTOs = new List<CourseDTO>();
            var dbCourseByUserID = (from c in _dataContext.Courses
                                    join uc in _dataContext.UserCourses
                                    on c.Id equals uc.CoursesId
                                    where uc.UserId == userID
                                    select c).ToListAsync();

            courseDTOs = dbCourseByUserID.Result.Select(c => _mapper.Map<CourseDTO>(c)).ToList();

            serviceResponse.Data = PageList<CourseDTO>.ToPageList(
            courseDTOs.AsEnumerable<CourseDTO>()/*.OrderBy(on => on.Courseid)*/,
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<Course>> GetCourseByCourseID(int courseID)
        {
            var serviceResponse = new ServiceResponse<Course>();
            var dbCourse = await _dataContext.Courses.FirstOrDefaultAsync(c => c.Id == courseID);

            if (dbCourse == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            serviceResponse.Data = dbCourse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<CourseDTO>> UpdateCourse(CreateCourseDTO updateCourseDto, int courseID)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();

            var courseRespone = await GetCourseByCourseID(courseID);

            if (courseRespone.Status == false)
            {
                serviceResponse.updateResponse(courseRespone.StatusCode, courseRespone.Message);

                return serviceResponse;
            }

            var course = courseRespone.Data;

            course.FullName = updateCourseDto.FullName;
            course.ShortName = updateCourseDto.ShortName;
            course.StartDate = updateCourseDto.StartDate;
            course.EndDate = updateCourseDto.EndDate;
            course.Description = updateCourseDto.Description;

            _dataContext.Courses.Update(course);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Sửa thành công!";
            serviceResponse.Data = _mapper.Map<CourseDTO>(course);

            return serviceResponse;
        }

        public async Task<ServiceResponse<CourseDTO>> DeleteCourse(int courseID, Course course)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();

            var studentRespone = await _dataContext.UserCourses.Where(x => x.CoursesId == courseID ).ToListAsync();

            if (studentRespone.Any())
            {
                serviceResponse.updateResponse(400, "Đang tồn tại học viên!");

                return serviceResponse;
            }

            await DeleteRelationshipCourse(course.Id);

            course.IsDeleted = 1;
            _dataContext.Courses.Update(course);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Xóa thành công!";

            return serviceResponse;
        }

        public async Task<UserCourse> CreateUserCourse(int userID, int courseID)
        {
            UserCourseDTO userCourseDto = new UserCourseDTO(userID, courseID);

            UserCourse userCourseSaved = _mapper.Map<UserCourse>(userCourseDto);
            _dataContext.UserCourses.Add(userCourseSaved);

            await _dataContext.SaveChangesAsync();
            return userCourseSaved;
        }

        private async Task DeleteRelationshipCourse(int courseID)
        {
            var userCourseByCourse = await _dataContext.UserCourses.Where(x => x.CoursesId == courseID).ToListAsync();

            if (userCourseByCourse.Any())
            {
                userCourseByCourse.ForEach(uc => uc.IsDeleted = 1);
                _dataContext.UserCourses.UpdateRange(userCourseByCourse);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
