using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.QuizService
{
    public interface IQuizService
    {
        Task<ServiceResponse<QuizResponseDTO>> createNewQuiz(CreateQuizDTO createQuizDTO);
        Task<ServiceResponse<PageList<QuizDTO>>> getAllQuiz(OwnerParameter ownerParameters);
        Task<ServiceResponse<QuizDetailResponseDTO>> getQuizById(int id);
        Task<ServiceResponse<QuizDTO>> deleteQuizz(int id);
        Task<ServiceResponse<QuizResponseDTO>> updateQuizz(CreateQuizDTO updateQuizDTO, int id);
        Task<ServiceResponse<QuizQuestionDTO>> addQuestionIntoQuiz(CreateQuizQuestionDTO createQuizQuestionDTO);
    }
}
