using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<ServiceResponse<PageList<CategoryDTO>>> getAllCategory(OwnerParameter ownerParameters);
        Task<ServiceResponse<CategoryDTO>> getCategoryByID(int categoryID);
        Task<ServiceResponse<CategoryDTO>> createNewCategory(CreateCategoryDTO createCategory);
        Task<ServiceResponse<CategoryDTO>> updateCategory(int id, CreateCategoryDTO createCategory);
        Task<ServiceResponse<CategoryDTO>> deleteCategory(int id);
    }
}
