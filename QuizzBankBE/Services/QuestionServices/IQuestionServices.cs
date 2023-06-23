using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;


namespace QuizzBankBE.Services.QuestionServices
{
    public interface IQuestionServices
    {
        Task<ServiceResponse<QuestionResponseDTO>> createNewQuestion(CreateQuestionDTO createQuestionDTO);
    }
}
