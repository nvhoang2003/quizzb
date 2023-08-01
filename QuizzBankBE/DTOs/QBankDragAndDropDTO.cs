using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QBankDragAndDropDTO : QuestionDTO
    {
        public int Id { get; set; }

        public List<QuestionBankAnswerDTO> Answers { get; set; }

        public string Questionstype { get; set; }
    }

    public class CreateQBankDragAndDropDTO
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

        [IdExistValidation<Category>("Id")]
        public int CategoryId { get; set; }

        public int? AuthorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }

        public sbyte IsShuffle { get; set; }

        public virtual ICollection<QbTagDTO> QbTags { get; set; }

        [RegularExpression("^DragAndDropIntoText$", ErrorMessage = "The Question Type must be equal to 'DragAndDropIntoText'")]
        public string Questionstype { get; set; }

        public List<Choice> Choice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach(var choice in Choice)
            {
                if (!Content.Contains("[[" + choice.Position + "]]"))
                {
                    yield return new ValidationResult("End Date must be after the Start Date.", new[] { "EndDate.[" + choice.Position + "]" });
                }
            }
        }
    }

    public class Choice
    {
        public int Position { get; set; }

        public QuestionBankAnswerDTO Answer { get; set; }
    }
}
