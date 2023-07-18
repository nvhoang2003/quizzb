using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankMultipeChoiceDTO : QuestionDTO
    {
        public int Id { get; set; }

        [IdExistValidation<Category>("ID")]
        public int CategoryId { get; set; }

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        //[AnswerValidation<>]
        public virtual ICollection<QuestionBankAnswerDTO> Answers { get; set; }

        public string Questionstype = "MultiChoice";
    }

    public class QuestionBankMultipeChoiceResponseDTO : QuestionDTO
    {
        public int Id { get; set; }

        public CategoryDTO Category { get; set; }

        public sbyte? IsPublic { get; set; }

        public virtual ICollection<QuestionBankAnswerDTO> Answers { get; set; }

        public string Questionstype = "MultiChoice";
    }

    public class QuestionBankAnswerDTO : AnswerDTO
    {
        [Required]
        [IdExistValidation<QuizBank>("ID")]
        public int? QuizBankId { get; set; }
    }
}
