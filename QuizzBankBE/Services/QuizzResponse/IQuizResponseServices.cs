using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.QuizzResponse
{
    public interface IQuizResponseServices
    {
        Task<ServiceResponse<QuizResponseDetailDTO>> GetResponseDetail(int accessID);

        Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> GetListResponseForDoQuiz(OwnerParameter ownerParameter, int userIdLogin, int? quizId, int? courseId, DateTime? timeStart, DateTime? timeEnd);

        Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> GetListResponseForWriteQuiz(OwnerParameter ownerParameter, int? quizId, int? courseId, string? name);
    }
}
