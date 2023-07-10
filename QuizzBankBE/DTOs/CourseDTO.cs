using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace QuizzBankBE.DTOs
{
    public class CreateCourseDTO : BaseCourseDTO
    {
    }

    public class CourseDTO : BaseCourseDTO
    {
        public int Id { get; set; }
        public int? CreateBy { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int IsDeleted { get; set; }
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
        [IdExistValidation("user_courses", "userId")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation("user_courses", "coursesId")]
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
