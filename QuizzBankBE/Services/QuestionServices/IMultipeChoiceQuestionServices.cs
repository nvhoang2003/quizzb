using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IMultipeChoiceQuestionServices
    {
        Task<ServiceResponse<MultiQuestionDTO>> CreateNewMultipeQuestion(List<CreateMultiQuestionDTO> createQuestionDTO);

        Task<ServiceResponse<MultiQuestionDTO>> GetMultipeQuestionById(int id);

        Task<ServiceResponse<MultiQuestionDTO>> DeleteMultipeQuestion(int id);
    }
}
