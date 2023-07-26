using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public abstract class QuestionDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required]
        [StringLength(Const.MediumText)] //mediumtext 16 mib
        public string Content { get; set; }

        [Required]
        [EnumDataType(typeof(QuestionType))]
        public string Questionstype { get; set; }

        public string? Generalfeedback { get; set; }//phan hoi chung

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        [IdExistValidation<Category>("ID")]
        public int CategoryId { get; set; }

        public int? AuthorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }

        public sbyte IsShuffle { get; set; }

        public virtual ICollection<QbTagDTO> QbTags { get; set; }

        public enum QuestionType
        {
            MultiChoice,
            TrueFalse,
            Match,
            ShortAnswer,
            Numerical,
            Essay,
            Calculated,
            CalculatedMultichoice,
            DragAndDropIntoText,
            DragAndDropIntoMaker,
            DragAndDropOntoImage,
            EmbbedAnswer,
            RandomShortAnswerMatching,
        }
    }
}
