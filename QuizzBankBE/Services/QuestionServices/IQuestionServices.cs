using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;


namespace QuizzBankBE.Services.QuestionServices
{
    public interface IQuestionServices
    {
        Task<ServiceResponse<QuestionResponseDTO>> createNewQuestion(CreateQuestionDTO createQuestionDTO);

        Task<ServiceResponse<PageList<QuestionBankEntryResponseDTO>>> getListQuestion(OwnerParameter ownerParameters, int questionCategoryId);

        Task<ServiceResponse<QuestionBankEntryResponseDTO>> getQuestionById(int questionbankEntryId);

        Task<ServiceResponse<QuestionResponseDTO>> updateQuestion(CreateQuestionDTO createQuestionDTO, int id);

        Task<ServiceResponse<QuestionResponseDTO>> deleteQuestion(int id);
    }
}
