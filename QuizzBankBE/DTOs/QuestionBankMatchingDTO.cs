using Microsoft.AspNetCore.Http;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankMatchingDTO : QuestionDTO
    {
        public virtual ICollection<CreateMatchSubQuestionBankDTO> MatchSubs { get; set; }
    }

    public class CreateMatchSubQuestionBankDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string? QuestionText { get; set; }

        [Required]
        [StringLength(Const.String)]
        public string? AnswerText { get; set; }
    }
}
