using Microsoft.AspNetCore.Http;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankMatchingDTO : QuestionDTO
    {
        [IdExistValidation<Category>("ID")]
        public int CategoryId { get; set; }

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        public virtual ICollection<CreateMatchSubQuestionBankDTO> Answers { get; set; }

        public string Questionstype = "Matching";
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
