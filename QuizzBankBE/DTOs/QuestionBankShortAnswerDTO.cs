using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionBankShortAnswerDTO : QuestionDTO
    {
        public int Id { get; set; }
        public List<QuestionBankAnswerDTO> Answers { get; set; }

        [RegularExpression("^ShortAnswer$", ErrorMessage = "The Question Type must be equal to 'ShortAnswer'")]
        public override string Questionstype { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class CreateQuestionBankShortAnswerDTO : QuestionDTO
    {
        [RegularExpression("^ShortAnswer$", ErrorMessage = "The Question Type must be equal to 'ShortAnswer'")]
        public override string Questionstype { get; set; }
        public List<QuestionBankAnswerDTO> Answers { get; set; }
    }
}
