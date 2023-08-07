using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public class TrueFalseQuestionDTO :BaseQuestionDTO
    {
        public List<QuestionAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }
    }

    public class CreateQuestionTrueFalseDTO : BaseQuestionDTO
    {
        [RegularExpression("^TrueFalse$", ErrorMessage = "The Question Type must be equal to 'TrueFalse'")]
        public string Questionstype { get; set; }
        public bool RightAnswer { get; set; }
    }
}
