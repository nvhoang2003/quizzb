using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IShortAnswerQuestionServices
    {
        Task<ServiceResponse<ShortAnswerQuestionDTO>> createSAQuestion(CreateShortAnswerQuestionDTO createQuestionBankDTO);

        Task<ServiceResponse<ShortAnswerQuestionDTO>> getSAQuestionById(int id);

        Task<ServiceResponse<ShortAnswerQuestionDTO>> deleteSAQuestion(int id);
    }
}
