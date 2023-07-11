using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.TagServices
{
    public interface ITagServices
    {
        Task<ServiceResponse<TagResponseDTO>> createNewTag(CreateTagDTO createTagDTO);
        Task<ServiceResponse<PageList<TagDTO>>> getAllTag(OwnerParameter ownerParameters);
        Task<ServiceResponse<TagDTO>> updateTag(CreateTagDTO updateTagDTO, int id);
        Task<ServiceResponse<TagDTO>> deleteTag(int id);
    }
}
