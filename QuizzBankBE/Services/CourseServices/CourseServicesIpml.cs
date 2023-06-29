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

namespace QuizzBankBE.Services.CourseServices
{
    public class CourseServicesIpml : ICourseServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public CourseServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<CourseDTO>> createCourse(BaseCourseDTO createCourseDto, int userIdLogin)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();
            var courseSaved = _mapper.Map<Course>(createCourseDto);

            _dataContext.Courses.Add(courseSaved);
            await _dataContext.SaveChangesAsync();

            var userCourseSaved = await createUserCourse(userIdLogin, courseSaved.Courseid);

            serviceResponse.Message = "Tạo thành công !";
            serviceResponse.Data = _mapper.Map<CourseDTO>(courseSaved);

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<CourseDTO>>> getAllCourse(OwnerParameter ownerParameters)
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

        public async Task<ServiceResponse<PageList<CourseDTO>>> getAllCourseByUserID(OwnerParameter ownerParameters, int userID)
        {
            var serviceResponse = new ServiceResponse<PageList<CourseDTO>>();
            var courseDTOs = new List<CourseDTO>();
            var dbCourseByUserID = (from c in _dataContext.Courses
                                    join uc in _dataContext.UserCourses
                                    on c.Courseid equals uc.CoursesId
                                    where uc.UserId == userID
                                    select c).ToListAsync();

            courseDTOs = dbCourseByUserID.Result.Select(c => _mapper.Map<CourseDTO>(c)).ToList();

            serviceResponse.Data = PageList<CourseDTO>.ToPageList(
            courseDTOs.AsEnumerable<CourseDTO>()/*.OrderBy(on => on.Courseid)*/,
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<Course>> getCourseByCourseID(int courseID)
        {
            var serviceResponse = new ServiceResponse<Course>();
            var dbCourse = await _dataContext.Courses.FirstOrDefaultAsync(c => c.Courseid == courseID);

            if (dbCourse == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            serviceResponse.Data = dbCourse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<CourseDTO>> updateCourse(BaseCourseDTO updateCourseDto,int courseID)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();

            var courseRespone = await getCourseByCourseID(courseID);

            if (courseRespone.Status == false)
            {
                serviceResponse.updateResponse(courseRespone.StatusCode, courseRespone.Message);

                return serviceResponse;
            }

            var course = courseRespone.Data;

            course.Fullname = updateCourseDto.Fullname;
            course.Shortname = updateCourseDto.Shortname;
            course.Startdate = updateCourseDto.Startdate;
            course.Enddate = updateCourseDto.Enddate;

            _dataContext.Courses.Update(course);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Sửa thành công!";
            serviceResponse.Data = _mapper.Map<CourseDTO>(course);

            return serviceResponse;
        }

        public async Task<ServiceResponse<CourseDTO>> deleteCourse(int courseID)
        {
            var serviceResponse = new ServiceResponse<CourseDTO>();

            var courseRespone = await getCourseByCourseID(courseID);

            if (courseRespone.Status == false)
            {
                serviceResponse.updateResponse(courseRespone.StatusCode, courseRespone.Message);

                return serviceResponse;
            }

            var course = courseRespone.Data;

            await deleteRelationshipCourse(course.Courseid);

            course.IsDeleted = 1;
            _dataContext.Courses.Update(course);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Xóa thành công!";

            return serviceResponse;
        }

        public async Task<UserCourse> createUserCourse(int userID, int courseID)
        {
            UserCourseDTO userCourseDto = new UserCourseDTO(userID, courseID, UserCourseRole.Admin.ToString());

            UserCourse userCourseSaved = _mapper.Map<UserCourse>(userCourseDto);
            _dataContext.UserCourses.Add(userCourseSaved);

            await _dataContext.SaveChangesAsync();
            return userCourseSaved;
        }

        private async Task deleteRelationshipCourse(int courseID)
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
