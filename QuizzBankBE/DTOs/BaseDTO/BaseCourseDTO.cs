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
        public string Fullname { get; set; } = null!;
        [Required]
        [StringLength(20)]
        public string Shortname { get; set; } = null!;
        [Required]
        public DateTime? Startdate { get; set; }
        [Required]
        public DateTime? Enddate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Startdate.Value <= Enddate.Value)
            {
                yield return new ValidationResult("End Date must be greater than the Start Date.", new[] { "Enddate" });
            }

          
        }
    }
}
