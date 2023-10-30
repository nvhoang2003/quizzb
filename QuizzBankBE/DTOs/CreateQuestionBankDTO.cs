using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required]
        [StringLength(Const.MediumText)] //mediumtext 16 mib
        public string Content { get; set; }

        [EnumDataType(typeof(QuestionType))]
        public string Questionstype { get; set; }

        [Required]
        [StringLength(Const.MediumText)]
        public string? Generalfeedback { get; set; }//phan hoi chung

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        [IdExistValidation<Category>("Id")]
        public int CategoryId { get; set; }

        public int? AuthorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }

        public sbyte? IsShuffle { get; set; }

        public ICollection<CreateQuestionBankAnswerDTO>? QuizbankAnswers { get; set; }

        public bool? RightAnswer { get; set; }

        public ICollection<CreateMatchSubQuestionBank>? MatchSubQuestionBanks { get; set; }

        public virtual ICollection<QbTagDTO>? QbTags { get; set; }

        public IFormFile? ImageFile { get; set; }

        public enum QuestionType
        {
            MultiChoice,
            TrueFalse,
            Match,
            ShortAnswer,
            DragAndDropIntoText,
        }

        public int? FileId { get; set; }
    }

    public class QuestionBankResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }

        public string Questionstype { get; set; }

        public string? Generalfeedback { get; set; }//phan hoi chung

        public sbyte? IsPublic { get; set; }

        public int CategoryId { get; set; }

        public int? AuthorId { get; set; }

        public float DefaultMark { get; set; }

        public sbyte? IsShuffle { get; set; }

        public ICollection<CreateQuestionBankAnswerDTO>? QuizbankAnswers { get; set; }

        public ICollection<CreateMatchSubQuestionBank>? MatchSubQuestionBanks { get; set; }

        public virtual ICollection<TagDTO> Tags { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class CreateQuestionBankAnswerDTO
    {
        public int? Id { get; set; }
        public int? Position { get; set; }

        [Required]
        [StringLength(Const.MediumText)]
        public string Content { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Fraction must be between 0% and 100%.")]
        public float Fraction { get; set; }

        [StringLength(Const.String)]
        public string? Feedback { get; set; }

        public int? QuizBankId { get; set; }
        public int? QuestionId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? FileId { get; set; }
        public string? ImageUrl { get; set; }
        public SystemFileResponseDTO? SystemFile { get; set; }
    }

    public class CreateMatchSubQuestionBank
    {
        public int Id { get; set; }

        public int? QuestionBankId { get; set; }

        public string? QuestionText { get; set; }

        public string? AnswerText { get; set; }

        public IFormFile? ImageFile { get; set; }

        public int? FileId { get; set; }
        public string? ImageUrl { get; set; }

        public SystemFileResponseDTO? SystemFile { get; set; }

    }
}
