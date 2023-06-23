using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.DTOs
{
    public class QuestionDTO
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Questionstype { get; set; }
        public string? Generalfeedback { get; set; }
        public int Createdby { get; set; }
        public int Updatedby { get; set; }
        public float DefaultMark { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public class QuestionResponseDTO {
        public virtual QuestionDTO question {get; set;}
        public virtual QuestionBankEntry questionBankEntry { get; set; }
        public string Status { get; set; }
    }
}
