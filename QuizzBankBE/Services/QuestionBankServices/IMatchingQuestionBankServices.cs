using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IMatchingQuestionBankServices
    {
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> CreateMatchingQuestionBank(CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> GetMatchSubsQuestionBankById(int questionBankID);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> UpdateMatchSubsQuestionBank(CreateQuestionBankMatchingDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<QuestionBankMatchingResponseDTO>> DeleteMatchSubsQuestionBank(int questionBankID);
    }
}
