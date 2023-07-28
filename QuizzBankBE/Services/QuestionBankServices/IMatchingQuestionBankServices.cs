using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IMatchingQuestionBankServices
    {
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> createMatchingQuestionBank(CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> getMatchSubsQuestionBankById(int questionBankID);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> updateMatchSubsQuestionBank(CreateQuestionBankMatchingDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> deleteMatchSubsQuestionBank(int questionBankID);
    }
}
