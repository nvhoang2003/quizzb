using static QuizzBankBE.DTOs.CourseDTO;
using System.ComponentModel.DataAnnotations;
using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseCourseDTO
    {
        [Required]
        [StringLength(255)]
        public string Fullname { get; set; } = null!;
        [Required]
        [StringLength(255)]
        public string Shortname { get; set; } = null!;

        public DateTime? Startdate { get; set; }

        public DateTime? Enddate { get; set; }
    }
}
