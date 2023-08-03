using QuizzBankBE.DTOs.QuestionBankDTOs;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public class ShortAnswerQuestionDTO : BaseQuestionDTO
    {
        public int Id { get; set; }

        public List<QuestionAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }
    }

    public class CreateShortAnswerQuestionDTO : BaseQuestionDTO
    {
        public List<QuestionAnswerDTO> Answers { get; set; }

        [RegularExpression("^ShortAnswer$", ErrorMessage = "The Question Type must be equal to 'ShortAnswer'")]
        public string Questionstype { get; set; }
    } 
}
