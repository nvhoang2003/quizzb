using static QuizzBankBE.DTOs.CourseDTO;
using System.ComponentModel.DataAnnotations;
using QuizzBankBE.DataAccessLayer.DataObject;
using System;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseCourseDTO
    {
        [Required]
        [StringLength(255)]
        public string FullName { get; set; } = null!;
        [Required]
        [StringLength(20)]
        public string ShortName { get; set; } = null!;
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [StringLength(16 * 1024 * 1024)]
        public string? Description { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.Value <= EndDate.Value)
            {
                yield return new ValidationResult("End Date must be greater than the Start Date.", new[] { "Enddate" });
            }
        }
    }
}
