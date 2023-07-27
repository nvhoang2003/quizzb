using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class AnswerDTO
    {
        [Required]
        [StringLength(Const.MediumText)]
        public string Content { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Fraction must be between 0% and 100%.")]
        public float Fraction { get; set; }

        [StringLength(Const.String)]
        public string? Feedback { get; set; }

        public int? QuizBankId { get; set; }
    }

   
}
