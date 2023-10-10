using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface ITrueFalseQuestionService
    {
        Task<ServiceResponse<TrueFalseQuestionDTO>> CreateNewTrueFalseQuestion(CreateQuestionTrueFalseDTO createQuestionTFDTO);
        Task<ServiceResponse<TrueFalseQuestionDTO>> GetTrueFalseQuestionById(int Id);
        Task<ServiceResponse<TrueFalseQuestionDTO>> UpdateTrueFalseQuestion(CreateQuestionTrueFalseDTO updateQbTrueFalseDTO, int id);
        Task<ServiceResponse<TrueFalseQuestionDTO>> DeleteTrueFalseQuestion(int id);
    }
}
