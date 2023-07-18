using System.ComponentModel.DataAnnotations;
using QuizzBankBE.FormValidator;

//namespace QuizzBankBE.DTOs.BaseDTO
//{
//    public abstract class BaseQuestionDTO
//    {
//        [Required]
//        [StringLength(255)]
//        public string Name { get; set; }

//        [Required]
//        [StringLength(16 * 1024 *1024)] //mediumtext 16 mib
//        public string Content { get; set; }

//        [Required]
//        [EnumDataType(typeof(QuestionType))]
//        public string Questionstype { get; set; }

//        public string? Generalfeedback { get; set; }//phan hoi chung

//        public int AuthorId { get; set; }

//        [Required]
//        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
//        public float DefaultMark { get; set; }
        
//        [AnswerValidation]
//        public virtual ICollection<QuestionAnswerDTO> Answers { get; set; }

//        public enum QuestionType
//        {
//            MultiChoice,
//            TrueFalse,
//            Match,
//            ShortAnswer,
//            Numerical,
//            Essay,
//            Calculated,
//            CalculatedMultichoice,
//            DragAndDropIntoText,
//            DragAndDropIntoMaker,
//            DragAndDropOntoImage,
//            EmbbedAnswer,
//            RandomShortAnswerMatching,
//        }
//    }
//}
