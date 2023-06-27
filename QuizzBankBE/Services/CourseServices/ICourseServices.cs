using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;

namespace QuizzBankBE.Services.CourseServices
{
    public interface ICourseServices
    {
        Task<ServiceResponse<CourseDTO>> createCourse(BaseCourseDTO createCourseDto, int userIdLogin);
        Task<ServiceResponse<PageList<CourseDTO>>> getAllCourse(OwnerParameter ownerParameters);
        Task<ServiceResponse<PageList<CourseDTO>>> getAllCourseByUserID(OwnerParameter ownerParameters, int userID);
        Task<ServiceResponse<Course>> getCourseByCourseID(int courseID);
        Task<ServiceResponse<CourseDTO>> updateCourse(BaseCourseDTO updateCourseDto, int userIdLogin);
    }
}
