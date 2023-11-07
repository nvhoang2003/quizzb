using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.QuizService
{
    public interface IQuizzAccessService
    {
        Task<ServiceResponse<QuizAccessDTO>> CreateQuizzAccess(CreateQuizAccessDTO createAccessDTO);
        Task<ServiceResponse<QuizAccessDTO>> UpdateStatus(CreateQuizAccessDTO updateStatusDTO, int id);
        Task<ServiceResponse<PageList<QuizAccessDTO>>> GetListQuizzAccess(OwnerParameter ownerParameters, int? courseId, string? studentName, string? status, bool? isPublic);
        Task<ServiceResponse<QuizAccessDTO>> DeleteQuizAccess(int id);
    }
}
