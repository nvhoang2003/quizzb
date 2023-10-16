using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IQuestionService
    {
        Task<ServiceResponse<QuestionResponseDTO>> GetQuestionById(int id);

        Task<ServiceResponse<QuestionResponseDTO>> DeleteQuestion(int id);
    }
}
