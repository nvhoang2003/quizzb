using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateCourseDTO : BaseCourseDTO
    {
        private DataContext _dataContext;

        [Required]
        [StringLength(255)]
        [UniqueValidation("GetDbSet", "FullName")]
        public string FullName { get; set; }

        [Required]
        [StringLength(20)]
        [UniqueValidation("GetDbSet", "ShortName")]
        public string ShortName { get; set; }

        public CreateCourseDTO()
        {
            _dataContext = new DataContext();
        }

        public IEnumerable<Course> GetDbSet()
        {
            return _dataContext.Set<Course>();
        }
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
        [IdExistValidation("users", "userId")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation("courses", "coursesId")]
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
