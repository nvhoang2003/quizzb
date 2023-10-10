using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface IQuestionServices
    {
        Task<ServiceResponse<DragAndDropQuestionDTO>> CreateQuestion(CreateDragAndDropDTO createQuestionBankMatchingDTO);
        Task<ServiceResponse<DragAndDropQuestionDTO>> GetQuestionById(int questionBankID);
        Task<ServiceResponse<DragAndDropQuestionDTO>> UpdateQuestion(CreateDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID);
        Task<ServiceResponse<DragAndDropQuestionDTO>> DeleteQuestion(int questionBankID);
    }
}
