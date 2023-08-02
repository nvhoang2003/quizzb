using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IMatchingQuestionServices
    {
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> createMatchingQuestionBank(CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> getMatchSubsQuestionBankById(int questionBankID);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> updateMatchSubsQuestionBank(CreateQuestionBankMatchingDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> deleteMatchSubsQuestionBank(int questionBankID);
    }
}
