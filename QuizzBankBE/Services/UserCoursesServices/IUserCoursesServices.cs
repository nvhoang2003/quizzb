using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.UserCoursesServices
{
    public interface IUserCoursesServices
    {
        Task<ServiceResponse<PageList<UserDTO>>> getListUserInCourse(OwnerParameter ownerParameters, int courseId);
        Task<ServiceResponse<UserCourseDTO>> addListUserIntoCourse(IEnumerable<UserCourseDTO> createUsersCourse);
        Task<ServiceResponse<UserCourseDTO>> removeUserFromCourses(int userId, int coursesId);
    }
}
