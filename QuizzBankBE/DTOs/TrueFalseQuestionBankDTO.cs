using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class TrueFalseQuestionBankDTO : QuestionDTO
    {
        public int Id { get; set; }

        public CategoryDTO Category { get; set; }

        public sbyte? IsPublic { get; set; }

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        public List<Tag> Tags { get; set; }

        public string Questionstype { get; set; }

    }

    public class TrueFalseQuestionDTO : QuestionDTO
    {
        

    }

    public class CreateTrueFalseQuestionDTO : QuestionDTO
    {
        public virtual ICollection<CreateAnswerTrueFalseDTO> Answers { get; set; }

        [RegularExpression("^TrueFalse", ErrorMessage = "The Question Type must be equal to 'TrueFalse'")]
        public string Questionstype { get; set; }
    }

    public class CreateAnswerTrueFalseDTO
    {
        public int id;
        [Required]
        public Boolean Content { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Fraction must be between 0% and 100%.")]
        public float Fraction { get; set; }

        [StringLength(Const.String)]
        public string? Feedback { get; set; }

        public int? QuizBankId { get; set; }

    }
}
