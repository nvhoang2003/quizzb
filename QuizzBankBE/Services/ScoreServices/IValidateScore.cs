using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.ScoreServices
{
    public interface IValidateScore
    {
        Task<Dictionary<string, List<string>>> checkAccessId(int accessId, int userLoginId);

        Task<Dictionary<string, List<string>>> checkListQuestion<T>(List<T> listQuestions) where T: DoQuestionDTO;
    }
}
