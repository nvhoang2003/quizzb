using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuizService
{
    public interface IQuizzAccessService
    {
        Task<ServiceResponse<QuizAccessDTO>> createQuizzAccess(CreateQuizAccessDTO createAccessDTO);
        Task<ServiceResponse<QuizAccessDTO>> updateStatus(CreateQuizAccessDTO updateStatusDTO, int id);


    }
}
