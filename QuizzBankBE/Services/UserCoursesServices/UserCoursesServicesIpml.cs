using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.UserCoursesServices
{
    public class UserCoursesServicesIpml : IUserCoursesServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public UserCoursesServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<UserCourseDTO>> addListUserIntoCourse(IEnumerable<UserCourseDTO> createUserCourse)
        {
            var serviceResponse = new ServiceResponse<UserCourseDTO>();
            IEnumerable<UserCourse> userCourses = _mapper.Map<IEnumerable<UserCourse>>(createUserCourse);

            _dataContext.UserCourses.AddRange(userCourses);
            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Thêm thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<UserDTO>>> getListUserInCourse(OwnerParameter ownerParameters, int courseId)
        {
            var serviceResponse = new ServiceResponse<PageList<UserDTO>>();

            var userInCourse = await (from u in _dataContext.Users
                                join uc in _dataContext.UserCourses on u.Id equals uc.UserId
                                join c in _dataContext.Courses on uc.CoursesId equals c.Id
                                where c.Id == courseId
                                select u
                                ).Select(u => _mapper.Map<UserDTO>(u)).ToListAsync();

            serviceResponse.Data = PageList<UserDTO>.ToPageList(
             userInCourse.AsEnumerable<UserDTO>(),
             ownerParameters.pageIndex,
             ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<UserCourseDTO>> removeUserFromCourses(int userId, int coursesId)
        {
            var serviceResponse = new ServiceResponse<UserCourseDTO>();
            var userCourse = _dataContext.UserCourses.FirstOrDefaultAsync(q => q.UserId == userId && q.CoursesId == coursesId).Result;

            if (userCourse == null)
            {
                serviceResponse.updateResponse(404, "Không Tồn Tại");
            }
            else
            {
                userCourse.IsDeleted = 1;
                _dataContext.UserCourses.Update(userCourse);
                await _dataContext.SaveChangesAsync();

                serviceResponse.updateResponse(200, "Xóa thành công");
            }

            return serviceResponse;
        }
    }
}
