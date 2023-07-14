using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseQuizAccessDTO
    {
        [Required]
        [IdExistValidation("users", "userId")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation("courses", "coursesId")]
        public int QuizId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? TimeStartQuiz { get; set; }
    }
}
