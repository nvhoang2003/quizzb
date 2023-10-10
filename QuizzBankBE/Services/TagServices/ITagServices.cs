using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.TagServices
{
    public interface ITagServices
    {
        Task<ServiceResponse<TagResponseDTO>> CreateNewTag(CreateTagDTO createTagDTO);
        Task<ServiceResponse<PageList<TagDTO>>> GetAllTagByCategoryID(OwnerParameter ownerParameters, int categoryID);
        Task<ServiceResponse<TagDTO>> GetTagByID(int tagID);
        Task<ServiceResponse<TagDTO>> UpdateTag(CreateTagDTO updateTagDTO, int id);
        Task<ServiceResponse<TagDTO>> DeleteTag(int id);
    }
}
