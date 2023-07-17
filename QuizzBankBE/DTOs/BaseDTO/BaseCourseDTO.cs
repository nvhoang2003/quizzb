using static QuizzBankBE.DTOs.CourseDTO;
using System.ComponentModel.DataAnnotations;
using QuizzBankBE.DataAccessLayer.DataObject;
using System;
using QuizzBankBE.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.Services.CategoryServices;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseCourseDTO
    {
        public string FullName { get; set; }

        public string ShortName { get; set; }

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
                yield return new ValidationResult("End Date must be greater than the Start Date.", new[] { "EndDate" });
            }
        }
    }
}
