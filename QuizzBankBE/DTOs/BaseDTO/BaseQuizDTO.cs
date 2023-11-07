using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseQuizDTO
    {
        [IdExistValidation<Course>("Id")]
        public int Courseid { get; set; }

        [Required(ErrorMessage = "Please enter course name")]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(Const.MediumText)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please add TimeOpen to the request.")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeOpen { get; set; }

        [Required(ErrorMessage = "Please add TimeClose to the request.")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeClose { get; set; }
        [Required]
        public string? TimeLimit { get; set; }
        [Required]
        public string? Overduehanding { get; set; }
        [Required]
        public string? PreferedBehavior { get; set; }

        [Required(ErrorMessage ="Please enter Point to Pass")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = ".0")]
        public float PointToPass { get; set; }

        [Required(ErrorMessage = "Please enter Max Point")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = ".0")]
        public float MaxPoint { get; set; }

        [Required]
        public string NaveMethod { get; set; } = null!;

        [Required]
        public sbyte IsPublic { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TimeOpen.Value <= TimeClose.Value)
            {
                yield return new ValidationResult("Time Close must be greater than the Time Open.", new[] { "TimeClose" });
            }

            if (PointToPass > MaxPoint) {

                yield return new ValidationResult("PointToPass must be less than the MaxPoint.", new[] { "PointToPass" });
            }
        }
    }
}
