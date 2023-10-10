using Microsoft.AspNetCore.Http;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public class CreateQuestionBankMatchingDTO : BaseCreateQuestionBankDTO
    {
        [RegularExpression("^Match$", ErrorMessage = "The Question Type must be equal to 'Match'")]
        public override string Questionstype { get; set; }

        public virtual List<CreateMatchSubQuestionBankDTO> MatchSubs { get; set; }
    }
    
    public class QuestionBankMatchingResponseDTO : BaseQuestionBankDTO
    {
        public int Id { get; set; }

        public List<MatchSubQuestionBankResponseDTO> MatchSubQuestions { get; set; }
        public List<String> MatchSubAnswers { get; set; }

        [RegularExpression("^Match$", ErrorMessage = "The Question Type must be equal to 'Match'")]
        public override string Questionstype { get; set; }
    }

    public class CreateMatchSubQuestionBankDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(Const.String)]
        public string AnswerText { get; set; }
    }

    public class MatchSubQuestionBankDTO : CreateMatchSubQuestionBankDTO
    {
        [Required]
        [IdExistValidation<MatchSubQuestionBank>("Id")]
        public int Id { get; set; }
    }

    public class MatchSubQuestionBankResponseDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
    }
}
