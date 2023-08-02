using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public class TrueFalseQuestionBankDTO : BaseQuestionBankDTO
    {
        public int Id { get; set; }

        public CategoryDTO Category { get; set; }

        public sbyte? IsPublic { get; set; }

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        public List<Tag> Tags { get; set; }

        [RegularExpression("^TrueFalse$", ErrorMessage = "The Question Type must be equal to 'TrueFalse'")]
        public string Questionstype { get; set; }

    }

    public class CreateTrueFalseQuestionDTO : BaseQuestionBankDTO
    {
        public List<QuestionBankAnswerDTO> Answers { get; set; }

        [RegularExpression("^TrueFalse", ErrorMessage = "The Question Type must be equal to 'TrueFalse'")]
        public string Questionstype { get; set; }
        public bool RightAnswer { get; set; }
    }
}
