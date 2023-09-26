using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;

namespace QuizzBankBE.Services.ListQuestionServices
{
    public interface IQuestionBankList
    {
        Task<ServiceResponse<PageList<ListQuestionBank>>> getListQuestionBank(OwnerParameter ownerParameters, int userLoginId, int? categoryId, string? name, string? author, string? questionType, string? tag, DateTime? startDate, DateTime? endDate);
        Task<ServiceResponse<PageList<ListQuestion>>> getListQuestion(OwnerParameter ownerParameters, int userLoginId, string? name, string? author, string? questionType, DateTime? startDate, DateTime? endDate);
        Task<ServiceResponse<Boolean>> createMultiQuestions(List<int> ids, int authorID);
    }
}
