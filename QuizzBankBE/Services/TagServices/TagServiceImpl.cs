using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.JWT;
using AutoMapper;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model.Pagination;
using Microsoft.EntityFrameworkCore;

namespace QuizzBankBE.Services.TagServices
{
    public class TagServicesIpml : ITagServices
    {
        public static DataContext _dataContext;
        public static IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        public TagServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public TagServicesIpml()
        {
        }
        public async Task<ServiceResponse<TagResponseDTO>> CreateNewTag(CreateTagDTO createTagDTO)
        {
            var serviceResponse = new ServiceResponse<TagResponseDTO>();
            Tag tagSaved = _mapper.Map<Tag>(createTagDTO);

            _dataContext.Tags.Add(tagSaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Tạo thành công");
            return serviceResponse;
        }
        public async Task<ServiceResponse<PageList<TagDTO>>> GetAllTagByCategoryID(OwnerParameter ownerParameters, int? categoryID)
        {
            var serviceResponse = new ServiceResponse<PageList<TagDTO>>();
            var dbTag = await _dataContext.Tags.ToListAsync();

            var tagDTO = dbTag.Select(u => _mapper.Map<TagDTO>(u)).Where(c => (categoryID == null || c.CategoryId.Equals(categoryID))).ToList();

            serviceResponse.Data = PageList<TagDTO>.ToPageList(
            tagDTO.AsEnumerable<TagDTO>(),

            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<TagDTO>> GetTagByID(int tagID)
        {
            var serviceResponse = new ServiceResponse<TagDTO>();
            var dbTag = await _dataContext.Tags.ToListAsync();

            var tagDTO = dbTag.Select(u => _mapper.Map<TagDTO>(u)).Where(c => c.Id == tagID).FirstOrDefault();
            if (tagDTO == null)
            {
                serviceResponse.updateResponse(400, "không tồn tại");
                return serviceResponse;
            }

            serviceResponse.Data = tagDTO;
            return serviceResponse;
        }

        public async Task<ServiceResponse<TagDTO>> UpdateTag(CreateTagDTO updateTagDTO, int id)
        {
            var serviceResponse = new ServiceResponse<TagDTO>();
            var dbTag = await _dataContext.Tags.FirstOrDefaultAsync(q => q.Id == id);
            if (dbTag == null)
            {
                serviceResponse.updateResponse(400, "Tag không tồn tại");
                return serviceResponse;
            }

            dbTag.Name = updateTagDTO.Name;
            dbTag.Description = updateTagDTO.Description;
            dbTag.CategoryId = updateTagDTO.CategoryId;

            _dataContext.Tags.Update(dbTag);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Update thành công");
            return serviceResponse;
        }
        public async Task<ServiceResponse<TagDTO>> DeleteTag(int id)
        {
            var serviceResponse = new ServiceResponse<TagDTO>();
            var dbTag = await _dataContext.Tags.FirstOrDefaultAsync(q => q.Id == id);
            if (dbTag == null)
            {
                serviceResponse.updateResponse(400, "Tag không tồn tại");
                return serviceResponse;
            }

            dbTag.IsDeleted = 1;
            _dataContext.Tags.Update(dbTag);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Delete thành công");

            return serviceResponse;
        }

    }
}
