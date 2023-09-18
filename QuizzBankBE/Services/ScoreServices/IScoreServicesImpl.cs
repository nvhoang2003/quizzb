using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.ScoreServices
{
    public interface IScoreServicesImpl
    {
        Task<ServiceResponse<float>> doQuestion(List<NewQuizResponse> newQuizResponses);
        Task<ServiceResponse<AllQuizzResponseDTO>> GetScore(int accessID);
    }
}
