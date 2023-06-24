using System.ComponentModel.DataAnnotations;
using QuizzBankBE.FormValidator;
using QuizzBankBE.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;


namespace QuizzBankBE.DTOs
{
    public class CreateQuestionDTO
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
        public virtual QuestionBankEntryDTO QuestionBankEntry { get; set; }

        public void SetUserMutation(int id_user_create, int id_user_modify)
        {
            this.Createdby = id_user_create;
            this.Updatedby = id_user_modify;
        }
        public enum QuestionType
        {
            MultipleChoice,
            TrueFalseQuestion,
            Matching,
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

    public class QuestionVersionDTO
    {
        [Required]
        public int QuestionId { get; set; }

        public int? QuestionBankEntryId { get; set; }

        [Required]
        [EnumDataType(typeof(QuestionVersionStatus))]
        public string Status { get; set; }

        public int Version { get; set; }

        public enum QuestionVersionStatus
        {
            Ready,
            Draft,
            Hidden,
        }

        public QuestionVersionDTO(int question_id, int question_bank_entry_id, string status, int version)
        {
            QuestionId = question_id;
            QuestionBankEntryId = question_bank_entry_id;
            Status = status;
            Version = version;
        }

        public QuestionVersionDTO()
        {

        }
    }

    public class QuestionBankEntryDTO
    {
        public QuestionBankEntryDTO(int questionCategoryId)
        {
            QuestionCategoryId = questionCategoryId;
        }

        [Required]
        [IdExistValidation("question_categories", "idquestion_categories")]
        public int QuestionCategoryId { get; set; }
    }
}
