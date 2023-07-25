using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionBankMatchingDTO
    {
        [IdExistValidation<Category>("ID")]
        public int CategoryId { get; set; }

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }
        public virtual ICollection<QuestionBankAnswerDTO> Answers { get; set; }

        public string Questionstype = "Matching";
    }
}
