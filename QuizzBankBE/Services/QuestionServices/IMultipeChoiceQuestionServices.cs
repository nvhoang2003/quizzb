using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IMultipeChoiceQuestionServices
    {
        Task<ServiceResponse<MultiQuestionDTO>> createNewMultipeQuestion(CreateMultiQuestionDTO createQuestionDTO);

        Task<ServiceResponse<MultiQuestionDTO>> getMultipeQuestionById(int id);

        Task<ServiceResponse<MultiQuestionDTO>> deleteMultipeQuestion(int id);
    }
}
