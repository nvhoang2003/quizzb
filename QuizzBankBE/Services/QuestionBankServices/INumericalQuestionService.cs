using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface INumericalQuestionService
    {
        Task<ServiceResponse<NumericalQuestionDTO>> CreateNumericalQuestionBank(CreateNumericalQuestionDTO createQuestionBankDTO);
        Task<ServiceResponse<NumericalQuestionDTO>> GetNumericalQuestionBankById(int id);
        Task<ServiceResponse<NumericalQuestionDTO>> DeleteNumericalQuestionBank(int id);
        Task<ServiceResponse<NumericalQuestionDTO>> UpdateNumericalQuestionBank(CreateNumericalQuestionDTO updateQbNumericalDTO, int id);
    }
}
