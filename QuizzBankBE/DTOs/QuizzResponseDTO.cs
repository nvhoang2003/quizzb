using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace QuizzBankBE.DTOs
{
    public class AllQuizzResponseDTO
    {
        public UserDTO userDoQuizz { get; set; }
        public CourseDTO course { get; set; }
        public QuizDTO quiz { get; set; }
        public QuizAccessDTO quizzAccess { get; set; }
        public float? totalPoint { get; set; } = 0;
        public string status { get; set; }

        public List<object> questionReults { get; set; } = new List<object>();
    }

    public class QuestionResultDTO
    {
        public object[] question { get; set; }
    }

    public class Do1QuizResponseDTO
    {
        public int Id { get; set; }

        public int? AccessId { get; set; }

        public float Mark { get; set; }

        public string? Status { get; set; }

        public int IsDeleted { get; set; }

        public int? QuestionId { get; set; }

        public String Answer { get; set; }
        public JsonElement AnswerToJson { get; set; }
    }

    public class QuizSubmmitDTO
    {
        [Required]
        [IdExistValidation<QuizAccess>("Id")]
        public int AccessId { get; set; }

        [Required]
        [IdExistValidation<Quiz>("Id")]
        public int QuizId { get; set; }

        public List<OneQuestionSubmitDTO>? listQuestionSubmit { get; set; } = new List<OneQuestionSubmitDTO>();
    }

    public class OneQuestionSubmitDTO
    {
        [Required]
        [IdExistValidation<Question>("Id")]
        public int? QuestionId { get; set; }

        [Required]
        public string QuestionType { get; set; }

        public List<int>? IdAnswerChoosen { get; set; }

        public string? ShortAnswerChoosen { get; set; }

        public List<MatchSubQuestionChoosenDTO>? MatchSubQuestionChoosen { get; set; }
    }

    public class MatchSubQuestionChoosenDTO
    {
        [Required]
        public string AnswerText { get; set; }

        [Required]
        public string QuestionText { get; set; }
    }
}
