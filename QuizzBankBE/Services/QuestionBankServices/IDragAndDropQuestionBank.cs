using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IDragAndDropQuestionBank
    {
        Task<ServiceResponse<QBankDragAndDropDTO>> CreateDragDropQuestionBank(CreateQBankDragAndDropDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<QBankDragAndDropDTO>> GetDragDropQuestionBankById(int questionBankID);
        Task<ServiceResponse<QBankDragAndDropDTO>> UpdateDragDropQuestionBank(CreateQBankDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<QBankDragAndDropDTO>> DeleteDragDropQuestionBank(int questionBankID);
    }
}
