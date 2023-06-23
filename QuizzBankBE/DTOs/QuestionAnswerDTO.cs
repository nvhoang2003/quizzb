using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionAnswerDTO
    {
        [Required]
        public string Content { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Fraction must be between 0% and 100%.")]
        public float Fraction { get; set; }
    }
}
