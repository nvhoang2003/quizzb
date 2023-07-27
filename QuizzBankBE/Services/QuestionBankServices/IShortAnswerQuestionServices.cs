using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IShortAnswerQuestionServices
    {
        Task<ServiceResponse<QuestionBankShortAnswerDTO>> createSAQuestionBank(CreateQuestionBankShortAnswerDTO createQuestionBankDTO);

        Task<ServiceResponse<QuestionBankShortAnswerDTO>> getSAQuestionBankById(int id);

        Task<ServiceResponse<QuestionBankShortAnswerDTO>> updateSAQuestionBank(CreateQuestionBankShortAnswerDTO createQuestionDTO, int id);

        Task<ServiceResponse<QuestionBankShortAnswerDTO>> deleteSAQuestionBank(int id);
    }
}
