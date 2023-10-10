using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IMultipeChoiceQuizBankServices
    {
        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> CreateNewMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO createQuestionBankDTO);

        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> GetMultipeQuestionBankById(int id);

        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> UpdateMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO createQuestionDTO, int id);

        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> DeleteMultipeQuestionBank(int id);
    }
}
