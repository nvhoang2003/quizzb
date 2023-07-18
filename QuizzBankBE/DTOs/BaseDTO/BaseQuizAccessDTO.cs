using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseQuizAccessDTO
    {
        [Required]
        [IdExistValidation<User>("Id")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation<Course>("Id")]
        public int QuizId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? TimeStartQuiz { get; set; }
    }
}
