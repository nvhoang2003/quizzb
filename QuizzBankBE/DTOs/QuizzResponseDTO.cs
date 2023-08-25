using QuizzBankBE.DTOs.QuestionDTOs;
using System.Text.Json;

namespace QuizzBankBE.DTOs
{
    public class AllQuizzResponseDTO
    {
        public UserDTO userDoQuizz { get; set; }
        public CourseDTO course { get; set; }
        public QuizDTO quiz { get; set; }
        public QuizAccessDTO quizzAccess { get; set; }

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

        public float? Mark { get; set; }

        public string? Status { get; set; }

        public int IsDeleted { get; set; }

        public int? QuestionId { get; set; }

        public String Answer { get; set; }
        public JsonElement AnswerToJson { get; set; }
    }
}
