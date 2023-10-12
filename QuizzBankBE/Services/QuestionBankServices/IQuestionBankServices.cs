using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface IQuestionBankServices
    {
        Task<ServiceResponse<CreateQuestionBankDTO>> CreateQuestionBank(CreateQuestionBankDTO createQuestionBankDTO);

        Task<ServiceResponse<QuestionBankResponseDTO>> GetQuestionBankById(int id);

        Task<ServiceResponse<CreateQuestionBankDTO>> UpdateQuestionBank(int id, CreateQuestionBankDTO createQuestionBankDTO);

        Task<ServiceResponse<CreateQuestionBankDTO>> DeleteQuestionBank(int id);
    }
}
