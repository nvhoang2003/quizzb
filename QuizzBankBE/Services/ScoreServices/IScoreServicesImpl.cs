using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.ScoreServices
{
    public interface IScoreServicesImpl
    {
        Task<ServiceResponse<float>> DoQuestion(List<NewQuizResponse> newQuizResponses, int accessId);
        Task<ServiceResponse<AllQuizzResponseDTO>> GetScore(int accessID);
    }
}
