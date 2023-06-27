using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using Microsoft.EntityFrameworkCore;

namespace QuizzBankBE.Services.KeywordServices
{
    public class KeywordServiceIpml : IKeywordService

    {

        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        public KeywordServiceIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public KeywordServiceIpml()
        {
        }

        public async Task<ServiceResponse<KeywordResponseDTO>> CreateNewKeyword(CreateKeywordDTO createKeywordDTO)
        {
            var serviceResponse = new ServiceResponse<KeywordResponseDTO>();


            Keyword keywordSaved = _mapper.Map<Keyword>(createKeywordDTO);

            _dataContext.Keywords.Add(keywordSaved);
            await _dataContext.SaveChangesAsync();
            serviceResponse.Status = true;
            serviceResponse.StatusCode = 200;
            serviceResponse.Message = "Tạo thành công !";
            return serviceResponse;
        }



        public async Task<ServiceResponse<PageList<KeywordDTO>>> getListKeywordByCourseID(OwnerParameter ownerParameters, int courseid)
        {
            var serviceResponse = new ServiceResponse<PageList<KeywordDTO>>();

            var dbKeyword = await _dataContext.Keywords.ToListAsync();
            var keywordDTO = dbKeyword.Select(u => _mapper.Map<KeywordDTO>(u)).
                Where(q => q.CourseId == courseid).ToList();


            serviceResponse.Data = PageList<KeywordDTO>.ToPageList(
            keywordDTO.AsEnumerable<KeywordDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<KeywordDTO>>> getAllKeyword(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<KeywordDTO>>();

            var dbKeyword = await _dataContext.Keywords.ToListAsync();
            var keywordDTO = dbKeyword.Select(u => _mapper.Map<KeywordDTO>(u)).ToList();


            serviceResponse.Data = PageList<KeywordDTO>.ToPageList(
            keywordDTO.AsEnumerable<KeywordDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }



        public async Task<ServiceResponse<CreateKeywordDTO>> updateKeyword( CreateKeywordDTO createKeywordDTO , int id)
        {
            var serviceResponse = new ServiceResponse<CreateKeywordDTO>();


            var dbKeyword = await _dataContext.Keywords.FirstOrDefaultAsync(q => q.Idkeywords == id);
            if (dbKeyword == null)
            {
                serviceResponse.updateResponse(400, "tu khoa không tồn tại");
                return serviceResponse;
            }


            dbKeyword.Content = createKeywordDTO.Content;
            _dataContext.Keywords.Update(dbKeyword);

            await _dataContext.SaveChangesAsync();


            serviceResponse.updateResponse(200, "Update thành công");

            return serviceResponse;
        }




        public async Task<ServiceResponse<KeywordDTO>> deleteKeyword(int id)
        {
            var serviceResponse = new ServiceResponse<KeywordDTO>();



            var dbKeyword = await _dataContext.Keywords.FirstOrDefaultAsync(q => q.Idkeywords == id);
            if (dbKeyword == null)
            {
                serviceResponse.updateResponse(400, "tu khoa không tồn tại");
                return serviceResponse;
            }


            dbKeyword.IsDeleted = 1;
            _dataContext.Keywords.Update(dbKeyword);

            await _dataContext.SaveChangesAsync();


            serviceResponse.updateResponse(200, "Delete thành công");

            return serviceResponse;
        }






    }
}
