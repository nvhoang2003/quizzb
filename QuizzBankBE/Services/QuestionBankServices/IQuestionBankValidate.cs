using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IQuestionBankValidate
    {
        Task<ServiceResponse<Dictionary<string, string>>> CheckValidate(CreateQuestionBankDTO createQuestionBankDTO);
    }
}
