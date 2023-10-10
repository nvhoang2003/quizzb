using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;

namespace QuizzBankBE.Services.ListQuestionServices
{
    public interface IQuestionBankList
    {
        Task<ServiceResponse<PageList<ListQuestionBank>>> GetListQuestionBank(OwnerParameter ownerParameters, int userLoginId, int? categoryId, string? name, string? author, string? questionType, string? tag, DateTime? startDate, DateTime? endDate, bool? isPublic);
        Task<ServiceResponse<PageList<ListQuestion>>> GetListQuestion(OwnerParameter ownerParameters, int userLoginId, string? name, string? author, string? questionType, DateTime? startDate, DateTime? endDate);
        Task<ServiceResponse<Boolean>> CreateMultiQuestions(List<int> ids);
        Task<ServiceResponse<Boolean>> DeleteQuestionBank(GeneralQuestionBankDTO ques);
        Task<ServiceResponse<Boolean>> DeleteQuestion(GeneralQuestionDTO ques);
    }
}
