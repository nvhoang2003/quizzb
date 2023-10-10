using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using Microsoft.AspNetCore.Mvc;

namespace QuizzBankBE.Services.CourseServices
{
    public interface ICourseServices
    {
        Task<ServiceResponse<CourseDTO>> CreateCourse(CreateCourseDTO createCourseDto, int userIdLogin);
        Task<ServiceResponse<PageList<CourseDTO>>> GetAllCourse(OwnerParameter ownerParameters);
        Task<ServiceResponse<PageList<CourseDTO>>> GetAllCourseByUserID(OwnerParameter ownerParameters, int userID);
        Task<ServiceResponse<Course>> GetCourseByCourseID(int courseID);
        Task<ServiceResponse<CourseDTO>> UpdateCourse(CreateCourseDTO updateCourseDto, int courseID);
        Task<ServiceResponse<CourseDTO>> DeleteCourse(int courseID, Course course);
    }
}
