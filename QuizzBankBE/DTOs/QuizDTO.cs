using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;

namespace QuizzBankBE.DTOs
{
    public class QuizDTO : BaseQuizDTO
    {
        public int Idquiz { get; set; }
        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }

    }



    public class QuizResponseDTO
    {
        public int IdquizVersions { get; set; }
        public virtual QuizDTO quiz { get; set; }
        public string Status { get; set; }
        public int Version { get; set; }
        public int? IsDeleted { get; set; }
    }


    public class CreateQuizDTO : BaseQuizDTO
    {

    }

   

}
