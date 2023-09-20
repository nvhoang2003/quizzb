using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.FormValidator;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuizDTO : BaseQuizDTO
    {
        public int ID { get; set; }
        public bool IsValid { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class QuizResponseDTO
    {
        public int IdquizVersions { get; set; }
        public virtual QuizDTO quiz { get; set; }
        public string Status { get; set; }
        public int Version { get; set; }
        public int? IsDeleted { get; set; }
    }

    public class QuizDetailResponseDTO : BaseQuizDTO
    {
        public int ID { get; set; }
        public bool IsValid { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<ListQuestion> listQuestion { get; set; } = new List<ListQuestion>();
    }
    public class CreateQuizDTO : BaseQuizDTO
    {
    }

    public class CreateQuizQuestionDTO
    {
        [IdExistValidation<Quiz>("Id")]
        public int? QuizzId { get; set; }
        public List<QuestionAdded> questionAddeds { get; set; }
    }

    public class QuizQuestionDTO
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? QuizzId { get; set; }
    }

    public class QuestionAdded
    {
        [IdExistValidation<Question>("Id")]
        public int? QuestionId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public float Point { get; set; }
    }

    public class QuizResponseForTest
    {
        public string  userName{ get; set; }
        public string courseName { get; set; }

        public QuizDTO quiz { get; set;  }

        public List<object> questionReults { get; set; } = new List<object>();
    }
}
