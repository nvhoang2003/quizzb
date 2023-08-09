using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public class CreateQuestionBankMultipeChoiceDTO : BaseQuestionBankDTO
    {
        [RegularExpression("^MultiChoice$", ErrorMessage = "The Question Type must be equal to 'MultiChoice'")]
        public override string Questionstype { get; set; }

        [AnswerValidation]
        public virtual ICollection<QuestionBankAnswerDTO> Answers { get; set; } 
    }

    public class QuestionBankMultipeChoiceResponseDTO : BaseQuestionBankDTO
    {
        public int Id { get; set; }

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        public override string Questionstype { get; set; }
    }

    public class QuestionBankAnswerDTO : AnswerDTO
    {
        public int Id { get; set; }

        public QuestionBankAnswerDTO(){}

        public QuestionBankAnswerDTO(float fraction, string content, int quizBankId) {
            this.Fraction = fraction;
            this.Content = content;
            this.QuizBankId = quizBankId;
        }
    }
}
