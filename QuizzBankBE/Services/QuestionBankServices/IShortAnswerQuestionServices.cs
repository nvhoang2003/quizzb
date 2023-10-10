using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IShortAnswerQuestionServices
    {
        Task<ServiceResponse<QuestionBankShortAnswerDTO>> CreateShortAnswerQuestionBank(CreateQuestionBankShortAnswerDTO createQuestionBankDTO);

        Task<ServiceResponse<QuestionBankShortAnswerDTO>> GetShortAnswerQuestionBankById(int id);

        Task<ServiceResponse<QuestionBankShortAnswerDTO>> UpdateShortAnswerQuestionBank(CreateQuestionBankShortAnswerDTO createQuestionDTO, int id);

        Task<ServiceResponse<QuestionBankShortAnswerDTO>> DeleteShortAnswerQuestionBank(int id);
    }
}
