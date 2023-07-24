using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseCourseDTO
    {
        [Required]
        [StringLength(Const.String)]
        [UniqueValidation<Course>("FullName")]
        public string FullName { get; set; }

        [Required]
        [StringLength(Const.MinString)]
        [UniqueValidation<Course>("ShortName")]
        public string ShortName { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [StringLength(Const.MediumText)]
        public string? Description { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.Value <= EndDate.Value)
            {
                yield return new ValidationResult("End Date must be after the Start Date.", new[] { "EndDate" });
            }
        }
    }
}
