using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuizService
{
    public interface IQuizzAccessService
    {
        Task<ServiceResponse<QuizAccessDTO>> CreateQuizzAccess(CreateQuizAccessDTO createAccessDTO);
        Task<ServiceResponse<QuizAccessDTO>> UpdateStatus(CreateQuizAccessDTO updateStatusDTO, int id);


    }
}
