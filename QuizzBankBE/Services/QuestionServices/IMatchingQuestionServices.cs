using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IMatchingQuestionServices
    {
        Task<ServiceResponse<MatchQuestionDTO>> CreateMatchingQuestion(CreateMatchQuestionDTO createQuestionMatchingDTO);
        Task<ServiceResponse<MatchQuestionDTO>> GetMatchSubsQuestionById(int questionBankID);
        Task<ServiceResponse<MatchQuestionDTO>> DeleteMatchSubsQuestion(int questionBankID);
    }
}
