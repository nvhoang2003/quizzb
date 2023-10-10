using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<ServiceResponse<PageList<CategoryDTO>>> GetAllCategory(OwnerParameter ownerParameters);
        Task<ServiceResponse<CategoryDTO>> GetCategoryByID(int categoryID);
        Task<ServiceResponse<CategoryDTO>> CreateNewCategory(CreateCategoryDTO createCategory);
        Task<ServiceResponse<CategoryDTO>> UpdateCategory(int id, CreateCategoryDTO createCategory);
        Task<ServiceResponse<CategoryDTO>> DeleteCategory(int id);
    }
}
