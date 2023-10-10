using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;
using static QuizzBankBE.DTOs.QuestionBankDTOs.BaseQuestionBankDTO;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface ITrueFalseQuestionBankService
    {
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> CreateNewTrueFalseQuestionBank(CreateTrueFalseQuestionDTO createQuestionTFDTO);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> GetTrueFalseQuestionBankById(int Id);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> UpdateTrueFalseQuestionBank(CreateTrueFalseQuestionDTO updateQbTrueFalseDTO, int id);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> DeleteTrueFalseQuestionBank(int id);
    }
}
