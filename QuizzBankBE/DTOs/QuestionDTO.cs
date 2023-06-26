using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionDTO : BaseQuestionDTO
    {
        public int Idquestions { get; set; }
    }

    public class QuestionResponseDTO
    {
        public int IdquestionVersions { get; set; }
        public virtual QuestionDTO question { get; set; }
        public virtual QuestionBankEntry questionBankEntry { get; set; }
        public string Status { get; set; }
        public int QuestionBankEntryId { get; set; }
        public int Version { get; set; }
    }

    public class QuestionBankEntryResponseDTO
    {
        public int IdquestionBankEntry { get; set; }
        public int QuestionCategoryId { get; set; }
        //public virtual QuestionCategoryDTO QuestionCategory { get; set; }
        public virtual QuestionDTO Question { get; set; }
        public virtual List<QuestionAnswerDTO> Answers { get; set; }
    }
}
