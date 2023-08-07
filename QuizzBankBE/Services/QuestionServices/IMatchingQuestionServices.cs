using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IMatchingQuestionServices
    {
        Task<ServiceResponse<MatchQuestionDTO>> createMatchingQuestion(CreateMatchQuestionDTO createQuestionMatchingDTO);
        Task<ServiceResponse<MatchQuestionDTO>> getMatchSubsQuestionById(int questionBankID);
        Task<ServiceResponse<MatchQuestionDTO>> deleteMatchSubsQuestion(int questionBankID);
    }
}
