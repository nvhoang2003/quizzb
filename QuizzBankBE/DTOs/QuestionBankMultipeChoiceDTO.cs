using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankMultipeChoiceDTO : QuestionDTO
    {
        [RegularExpression("^MultiChoice$", ErrorMessage = "The Question Type must be equal to 'MultiChoice'")]
        public string Questionstype { get; set; }

        [AnswerValidation]
        public virtual ICollection<QuestionBankAnswerDTO> Answers { get; set; }
    }

    public class QuestionBankMultipeChoiceResponseDTO : QuestionDTO
    {
        public int Id { get; set; }

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class QuestionBankAnswerDTO : AnswerDTO
    {
        public int Id { get; set; }

        public QuestionBankAnswerDTO(float fraction, string content, int quizBankId) {
            this.Fraction = fraction;
            this.Content = content;
            this.QuizBankId = quizBankId;
        }
    }
}
