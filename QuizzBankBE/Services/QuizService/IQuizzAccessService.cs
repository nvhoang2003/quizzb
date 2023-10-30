using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.QuizService
{
    public interface IQuizzAccessService
    {
        Task<ServiceResponse<QuizAccessDTO>> CreateQuizzAccess(CreateQuizAccessDTO createAccessDTO);
        Task<ServiceResponse<QuizAccessDTO>> UpdateStatus(CreateQuizAccessDTO updateStatusDTO, int id);

        Task<ServiceResponse<PageList<QuizAccessDTO>>> getListQuizzAccess(OwnerParameter ownerParameters, int? courseId, int? studentId);
    }
}
