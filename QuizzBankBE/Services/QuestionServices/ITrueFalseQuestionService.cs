using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface ITrueFalseQuestionService
    {
        Task<ServiceResponse<TrueFalseQuestionDTO>> createNewTrueFalseQuestion(CreateQuestionTrueFalseDTO createQuestionTFDTO);
        Task<ServiceResponse<TrueFalseQuestionDTO>> getTrueFalseQuestionById(int Id);
        Task<ServiceResponse<TrueFalseQuestionDTO>> updateTrueFalseQuestion(CreateQuestionTrueFalseDTO updateQbTrueFalseDTO, int id);
        Task<ServiceResponse<TrueFalseQuestionDTO>> deleteTrueFalseQuestion(int id);
    }
}
