using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IDragAndDropQuestion
    {
        Task<ServiceResponse<DragAndDropQuestionDTO>> createDDQuestion(CreateDragAndDropDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<DragAndDropQuestionDTO>> getDDQuestionById(int questionBankID);
        Task<ServiceResponse<DragAndDropQuestionDTO>> updateDDQuestion(CreateDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<DragAndDropQuestionDTO>> deleteDDQuestion(int questionBankID);
    }
}
