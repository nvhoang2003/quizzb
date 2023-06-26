using System.ComponentModel.DataAnnotations;
using QuizzBankBE.FormValidator;
using QuizzBankBE.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DTOs.BaseDTO;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionDTO : BaseQuestionDTO
    {
  
        [AnswerValidation]
        public virtual ICollection<QuestionAnswerDTO> Answers { get; set; }
        public virtual QuestionBankEntryDTO QuestionBankEntry { get; set; }
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
