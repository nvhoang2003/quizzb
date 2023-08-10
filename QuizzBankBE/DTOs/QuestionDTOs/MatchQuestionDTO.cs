using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public class MatchQuestionDTO : BaseQuestionDTO
    {
        public int Id { get; set; }

        public List<MatchSubQuestionResponseDTO> MatchSubQuestion { get; set; }

        public string Questionstype { get; set; }
    }

    public class CreateMatchQuestionDTO : BaseQuestionDTO
    {
        public List<CreateMatchSubQuestionDTO> MatchSubQuestion { get; set; }

        [RegularExpression("^Match$", ErrorMessage = "The Question Type must be equal to 'Match'")]
        public string Questionstype { get; set; }
    }

    public class MatchSubQuestionResponseDTO : CreateMatchSubQuestionDTO
    {
        [Required]
        [IdExistValidation<MatchSubQuestion>("Id")]
        public int Id { get; set; }
    }

    public class CreateMatchSubQuestionDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(Const.String)]
        public string AnswerText { get; set; }

        public int QuestionId { get; set; }
    }
}
