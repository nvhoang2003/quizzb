using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IShortAnswerQuestionServices
    {
        Task<ServiceResponse<ShortAnswerQuestionDTO>> CreateShortAnswerQuestion(CreateShortAnswerQuestionDTO createQuestionBankDTO);

        Task<ServiceResponse<ShortAnswerQuestionDTO>> GetShortAnswerQuestionById(int id);

        Task<ServiceResponse<ShortAnswerQuestionDTO>> DeleteShortAnswerQuestion(int id);
    }
}
