using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using static QuizzBankBE.DTOs.QuestionDTO;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public interface ITrueFalseQuestionBankService
    {
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> createNewTrueFalseQuestionBank(CreateTrueFalseQuestionDTO createQuestionTFDTO);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> getTrueFalseQuestionBankById(int Id);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> updateTrueFalseQuestionBank(CreateTrueFalseQuestionDTO updateQbTrueFalseDTO, int id);
        Task<ServiceResponse<TrueFalseQuestionBankDTO>> deleteTrueFalseQuestionBank(int id);
    }
}
