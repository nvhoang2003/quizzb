using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public abstract class DoQuestionDTO
    {
        [Required]
        [IdExistValidation<QuizAccess>("Id")]
        public int QuizAccessID { get; set; }
        [Required]
        [IdExistValidation<QuizBank>("Id")]
        public int QuestionID { get; set; }
        public abstract string Questionstype { get; set; }
    }

    public class DoMatchingDTO : DoQuestionDTO
    {
        [Required]
        [RegularExpression("^Match$", ErrorMessage = "The Question Type must be equal to 'Match$'")]
        public override string Questionstype { get; set; }
        public virtual List<MatchSubQuestionBankDTO> MatchSubs { get; set; }
    }
}
