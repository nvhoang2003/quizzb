using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model;
using static QuizzBankBE.DTOs.QuestionBankDTOs.BaseQuestionBankDTO;

namespace QuizzBankBE.Services.QuestionServices
{
    public interface ITrueFalseQuestionService
    {
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> createNewTrueFalseQuestionBank(CreateTrueFalseQuestionDTO createQuestionTFDTO);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> getTrueFalseQuestionBankById(int Id);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> updateTrueFalseQuestionBank(CreateTrueFalseQuestionDTO updateQbTrueFalseDTO, int id);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> deleteTrueFalseQuestionBank(int id);
    }
}
