using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankMultipeChoiceDTO : QuestionDTO
    {
        [IdExistValidation<Category>("Id")]
        public int CategoryId { get; set; }

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        //[AnswerValidation<>]
        public virtual ICollection<QuestionBankAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }

        public virtual ICollection<QbTagDTO> QbTags { get; set; }
    }

    public class QuestionBankMultipeChoiceResponseDTO : QuestionDTO
    {
        public int Id { get; set; }

        public CategoryDTO Category { get; set; }

        public sbyte? IsPublic { get; set; }

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class QuestionBankAnswerDTO : AnswerDTO
    {
        public int Id { get; set; }
    }
}
