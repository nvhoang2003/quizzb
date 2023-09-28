using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseQuizAccessDTO
    {
        [IdExistValidation<User>("Id")]
        public int UserId { get; set; }

        [Required]
        [IdExistValidation<Quiz>("Id")]
        public int QuizId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TimeStartQuiz { get; set; }
        public enum Status { 
            Wait,
            Doing,
            Complete
        }
    }
}
