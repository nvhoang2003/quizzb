using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IMultipeChoiceQuizBankServices
    {
        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> createNewMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO createQuestionBankDTO);

        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> getMultipeQuestionBankById(int id);

        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> updateMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO createQuestionDTO, int id);

        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> deleteMultipeQuestionBank(int id);
    }
}
