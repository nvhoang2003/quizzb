using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IMultipeChoiceQuizBankServices
    {
        Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> createNewMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO createQuestionBankDTO);

        //Task<ServiceResponse<QuestionBankEntryResponseDTO>> getMultipeQuestionBankById(int questionbankEntryId);

        //Task<ServiceResponse<QuestionResponseDTO>> updateMultipeQuestionBank(CreateQuestionDTO createQuestionDTO, int id);

        //Task<ServiceResponse<QuestionResponseDTO>> deleteMultipeQuestionBank(int id);
    }
}
