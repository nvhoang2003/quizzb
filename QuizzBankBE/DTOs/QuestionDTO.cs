using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionDTO
    {
        public int Idquestions { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Questionstype { get; set; }
        public string? Generalfeedback { get; set; }
        public int Createdby { get; set; }
        public int Updatedby { get; set; }
        public float DefaultMark { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
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
        //public virtual QuestionResponseDTO Question { get; set; }
        public virtual QuestionDTO Question { get; set; }
        public virtual List<QuestionAnswerDTO> Answers { get; set; }
    }
}
