using QuizzBankBE.DataAccessLayer.DataObject;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public class NumericalQuestionDTO : BaseQuestionBankDTO
    {
        public int Id { get; set; }
        public List<QuestionBankAnswerDTO> Answers { get; set; }

        [RegularExpression("^Numerical", ErrorMessage = "The Question Type must be equal to 'Numerical'")]
        public override string Questionstype { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class CreateNumericalQuestionDTO : BaseCreateQuestionBankDTO
    {

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        [RegularExpression("^Numerical", ErrorMessage = "The Question Type must be equal to 'Numerical'")]
        public override string Questionstype { get; set; }
        [Required]

        public int RightAnswers { get; set; }

    }
}
