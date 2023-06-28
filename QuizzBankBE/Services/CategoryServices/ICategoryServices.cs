using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<ServiceResponse<QuestionCategoryDTO>> createNewCategory(CreateQuestionCategoryDTO createCategory);

        Task<ServiceResponse<QuestionCategoryDTO>> getCategoryById(OwnerParameter ownerParameter, int id);
    }
}
