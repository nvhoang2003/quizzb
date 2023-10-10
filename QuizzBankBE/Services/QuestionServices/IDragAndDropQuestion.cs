using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IDragAndDropQuestion
    {
        Task<ServiceResponse<DragAndDropQuestionDTO>> CreateDragDropQuestion(CreateDragAndDropDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<DragAndDropQuestionDTO>> GetDragDropQuestionById(int questionBankID);
        Task<ServiceResponse<DragAndDropQuestionDTO>> UpdateDragDropQuestion(CreateDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<DragAndDropQuestionDTO>> DeleteDragDropQuestion(int questionBankID);
    }
}
