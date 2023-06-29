using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace QuizzBankBE.DTOs
{
    public class CourseDTO : BaseCourseDTO
    {
        public int Courseid { get; set; }
    }

    public class UserCourseDTO
    {
        public UserCourseDTO(int userId, int coursesId)
        {
            UserId = userId;
            CoursesId = coursesId;
        }

        public UserCourseDTO (int userId, int coursesId, string role)
        {
            UserId = userId;
            CoursesId = coursesId;
            Role = role;
        }

        [Required]
        [IdExistValidation("user_courses", "user_id")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation("user_courses", "courses_id")]
        public int CoursesId { get; set; }

        [Required]
        [EnumDataType(typeof(UserCourseRole))]
        public string Role { get; set; } = null!;

        public enum UserCourseRole
        {
            Admin,
            Teacher,
            Student
        }

        public enum PowerfullUserCourseRole
        {
            Admin,
            Teacher
        }

        public static bool checkPowerfullUserCourseRole(string role)
        {
            string[] powerfullUserCourseRoles = Enum.GetNames(typeof(PowerfullUserCourseRole));

            if (powerfullUserCourseRoles.Contains(role))
            {
                return true;
            }

            return false;
        }
    }
}
