using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseQuestionbankDTO
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(2 * 1024 * 1024)]
        public string Content { get; set; }
        [Required]
        [EnumDataType(typeof(QuestionType))]
        public string QuestionsType { get; set; }

        public string? GeneralFeedback { get; set; }
        [Required]
        [IdExistValidation("category", "ID")]
        public int CategoryId { get; set; }
        [Range(0,1)]
        public sbyte? IsPublic { get; set; }
        [Range(0, 1)]
        public sbyte IsShuffle { get; set; }

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
