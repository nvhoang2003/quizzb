using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface INumericalQuestionService
    {
        Task<ServiceResponse<NumericalQuestionDTO>> createNumericalQuestionBank(CreateNumericalQuestionDTO createQuestionBankDTO);
        Task<ServiceResponse<NumericalQuestionDTO>> getNumericalQuestionBankById(int id);
        Task<ServiceResponse<NumericalQuestionDTO>> deleteNumericalQuestionBank(int id);
        Task<ServiceResponse<NumericalQuestionDTO>> updateNumericalQuestionBank(CreateNumericalQuestionDTO updateQbNumericalDTO, int id);
    }
}
