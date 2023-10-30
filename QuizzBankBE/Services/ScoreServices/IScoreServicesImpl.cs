using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.ScoreServices
{
    public interface IScoreServicesImpl
    {
        Task<ServiceResponse<ResultQuizDTO>> DoQuestion(QuizSubmmitDTO newQuizResponses);
        Task<ServiceResponse<AllQuizzResponseDTO>> GetScore(int accessID);
    }
}
