using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.UserCoursesServices
{
    public interface IUserCoursesServices
    {
        Task<ServiceResponse<PageList<UserDTO>>> GetListUserInCourse(OwnerParameter ownerParameters, int courseId);
        Task<ServiceResponse<UserCourseDTO>> AddListUserIntoCourse(IEnumerable<UserCourseDTO> createUsersCourse);
        Task<ServiceResponse<UserCourseDTO>> RemoveUserFromCourses(int userId, int coursesId);
    }
}
