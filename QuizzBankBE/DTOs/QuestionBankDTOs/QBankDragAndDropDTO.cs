using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using ServiceStack;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public class QBankDragAndDropDTO : BaseQuestionBankDTO
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

        [CheckDragAndDropFields("Content")]
        public List<Choice> Choice { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    string pattern = @"\[\[(\d+)\]\]";

        //    MatchCollection matches = Regex.Matches(Content, pattern);
        //    foreach (Match match in matches)
        //    {
        //        int value = int.Parse(match.Groups[1].Value);
        //        bool check = Choice.Any(c => c.Position == value);
        //        if(check == false)
        //        {
        //            yield return new ValidationResult("Câu trả lời [[" + value + "]] cần có nội dung.", new[] { "Choice.[" + value + "]" });
        //        }
        //    }
        //}
    }

    public class Choice
    {
        public int Position { get; set; }

        public QuestionBankAnswerDTO Answer { get; set; }
    }
}
