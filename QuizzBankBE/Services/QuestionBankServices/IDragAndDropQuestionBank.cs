using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IDragAndDropQuestionBank
    {
        Task<ServiceResponse<QBankDragAndDropDTO>> createDDQuestionBank(CreateQBankDragAndDropDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<QBankDragAndDropDTO>> getDDQuestionBankById(int questionBankID);
        Task<ServiceResponse<QBankDragAndDropDTO>> updateDDQuestionBank(CreateQBankDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<QBankDragAndDropDTO>> deleteDDQuestionBank(int questionBankID);
    }
}
