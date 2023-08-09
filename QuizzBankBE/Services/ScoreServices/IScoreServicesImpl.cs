using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.ScoreServices
{
    public interface IScoreServicesImpl
    {
        Task<ServiceResponse<float>> doQuestion<T>(T doQuestionDTO) where T : DoQuestionDTO;
    }
}
