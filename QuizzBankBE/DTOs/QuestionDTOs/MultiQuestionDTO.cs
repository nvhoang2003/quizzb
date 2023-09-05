using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public class MultiQuestionDTO : BaseQuestionDTO
    {
        public int Id { get; set; }

        public List<QuestionAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }
    }

    public class CreateMultiQuestionDTO : BaseQuestionDTO
    {
        [RegularExpression("^MultiChoice$", ErrorMessage = "The Question Type must be equal to 'MultiChoice'")]
        public string Questionstype { get; set; }

        public virtual ICollection<QuestionAnswerDTO> QuestionAnswers { get; set; }
    }
}
