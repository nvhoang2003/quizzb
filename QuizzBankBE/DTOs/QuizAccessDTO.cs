using System.ComponentModel.DataAnnotations;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;

namespace QuizzBankBE.DTOs
{
    public class QuizAccessDTO : BaseQuizAccessDTO
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;

        public QuizDTO Quiz { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

    }

    public class CreateQuizAccessDTO : BaseQuizAccessDTO
    {
        public string Status { get; set; } = null!;

    }
}
