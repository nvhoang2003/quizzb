using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.TagServices
{
    public interface ITagServices
    {
        Task<ServiceResponse<TagResponseDTO>> createNewTag(CreateTagDTO createTagDTO);
        Task<ServiceResponse<PageList<TagDTO>>> getAllTagByCategoryID(OwnerParameter ownerParameters, int categoryID);
        Task<ServiceResponse<TagDTO>> updateTag(CreateTagDTO updateTagDTO, int id);
        Task<ServiceResponse<TagDTO>> deleteTag(int id);
    }
}
