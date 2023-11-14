using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuizDTO : BaseQuizDTO
    {
        public int ID { get; set; }
        public bool IsValid { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public CourseDTO? Course { get; set; }
    }
    public class QuizResponseDTO : BaseQuizDTO
    {
        public int ID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
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

    public class UpdateQuizPointDTO
    {
        [Required(ErrorMessage = "Please enter Point to Pass")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = ".0")]
        public float PointToPass { get; set; }

        [Required(ErrorMessage = "Please enter Max Point")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = ".0")]
        public float MaxPoint { get; set; }
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

        public List<QuestionResponseDTO> questionReults { get; set; } = new List<QuestionResponseDTO>();
    }
}
