using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuizDTO : BaseQuizDTO
    {
        public int ID { get; set; }
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
    public class CreateQuizDTO : BaseQuizDTO
    {
    }

    public class CreateQuizQuestionDTO
    {
        [IdExistValidation<Question>("Id")]
        public int? QuestionId { get; set; }

        [IdExistValidation<Quiz>("Id")]
        public int? QuizzId { get; set; }
    }

    public class QuizQuestionDTO
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? QuizzId { get; set; }
    }
}
