using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.DTOs
{
    public class QuestionResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string QuestionsType { get; set; } = null!;

        public string? GeneralFeedback { get; set; }

        public int? AuthorId { get; set; }

        public sbyte IsShuffle { get; set; }

        public float? DefaultMark { get; set; }

        public int? CreateBy { get; set; }

        public virtual ICollection<MatchSubQuestionResponseDTO> MatchSubQuestions { get; set; }

        public virtual ICollection<QuestionAnswerResponseDTO> QuestionAnswers { get; set; }
    }

    public class MatchSubQuestionResponseDTO
    {
        public int Id { get; set; }

        public int? QuestionId { get; set; }

        public string? QuestionText { get; set; }

        public string? AnswerText { get; set; }
    }

    public class QuestionAnswerResponseDTO
    {
        public int Id { get; set; }

        public int? QuestionId { get; set; }

        public string Content { get; set; } = null!;

        public float Fraction { get; set; }

        public string? Feedback { get; set; }
    }

    public class QuestionAnswerResultDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public float Fraction { get; set; }
        public int? IsDeleted { get; set; }
        public int QuestionId { get; set; }
        public bool? isChosen { get; set; }
    }
}
