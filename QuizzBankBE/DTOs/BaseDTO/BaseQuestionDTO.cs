using static QuizzBankBE.DTOs.CreateQuestionDTO;
using System.ComponentModel.DataAnnotations;
using QuizzBankBE.FormValidator;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseQuestionDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [EnumDataType(typeof(QuestionType))]
        public string Questionstype { get; set; }
        public string? Generalfeedback { get; set; }
        public int Createdby { get; set; }
        public int Updatedby { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }
        [AnswerValidation]
        public virtual ICollection<QuestionAnswerDTO> Answers { get; set; }

        public int? IsDeleted { get; set; }
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

        public void SetUserMutation(int id_user_create, int id_user_modify)
        {
            this.Createdby = id_user_create;
            this.Updatedby = id_user_modify;
        }
    }
}
