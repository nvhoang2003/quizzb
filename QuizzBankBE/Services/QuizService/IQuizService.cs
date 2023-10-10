using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.QuizService
{
    public interface IQuizService
    {
        Task<ServiceResponse<QuizResponseDTO>> CreateNewQuiz(CreateQuizDTO createQuizDTO);
        Task<ServiceResponse<PageList<QuizDTO>>> GetAllQuiz(OwnerParameter ownerParameters, string? name, DateTime? timeStart, DateTime? timeEnd, bool? isPublic, int? courseId);
        Task<ServiceResponse<QuizDetailResponseDTO>> GetQuizById(int id);
        Task<ServiceResponse<QuizDTO>> DeleteQuizz(int id);
        Task<ServiceResponse<QuizResponseDTO>> UpdateQuizz(CreateQuizDTO updateQuizDTO, int id);
        Task<ServiceResponse<QuizQuestionDTO>> AddQuestionIntoQuiz(CreateQuizQuestionDTO createQuizQuestionDTO);
        Task<ServiceResponse<QuizResponseForTest>> ShowQuizForTest(int id, string userName);
    }
}
