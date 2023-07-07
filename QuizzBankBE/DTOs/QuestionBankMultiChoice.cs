using QuizzBankBE.DTOs.BaseDTO;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionBankMultiChoiceCreate : BaseQuestionbankDTO
    {
        [EnumDataType(typeof(QuestionType))]
        public string Questionstype { get; set; }
    }
}
