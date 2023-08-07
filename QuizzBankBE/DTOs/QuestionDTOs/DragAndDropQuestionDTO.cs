using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public class DragAndDropQuestionDTO : BaseQuestionDTO
    {
        public int Id { get; set; }

        public List<QuestionAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }
    }

    public class CreateDragAndDropDTO : BaseQuestionDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required]
        [StringLength(Const.MediumText)]
        [RegularExpression(@".*\[\[\d+\]\].*", ErrorMessage = "Chuỗi phải chứa [[x]] với x là số")]
        public string Content { get; set; }

        public string? Generalfeedback { get; set; }

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        [IdExistValidation<User>("Id")]
        public int? AuthorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }

        public sbyte IsShuffle { get; set; }

        [RegularExpression("^DragAndDropIntoText$", ErrorMessage = "The Question Type must be equal to 'DragAndDropIntoText'")]
        public string Questionstype { get; set; }

        public List<QuestionChoice> Choice { get; set; }
    }

    public class QuestionChoice
    {
        public int Position { get; set; }

        public QuestionAnswerDTO Answer { get; set; }
    }
}
