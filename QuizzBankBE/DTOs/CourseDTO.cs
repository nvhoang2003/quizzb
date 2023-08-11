using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

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

    public class UserCourseDTO : IValidatableObject
    {
        public UserCourseDTO(int userId, int coursesId)
        {
            UserId = userId;
            CoursesId = coursesId;
        }

        [Required]
        [IdExistValidation<User>("Id")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation<Course>("Id")]
        public int CoursesId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DataContext _dataContext = new DataContext();
            if (_dataContext.UserCourses.Any(c => c.UserId == UserId && c.CoursesId == CoursesId))
            {
                yield return new ValidationResult("Học sinh đã được thêm vào trước đây", new[] { "student" });
            }
        }
    }
}
