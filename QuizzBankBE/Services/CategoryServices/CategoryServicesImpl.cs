using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.QuestionServices;

namespace QuizzBankBE.Services.CategoryServices
{
    public class CategoryServicesImpl : ICategoryServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IQuestionServices _questionServices;

        public CategoryServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public CategoryServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IQuestionServices questionServices)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _questionServices = questionServices;
        }

        public CategoryServicesImpl()
        {
        }

        public async Task<ServiceResponse<QuestionCategoryDTO>> createNewCategory(CreateQuestionCategoryDTO createCategory)
        {
            var serviceResponse = new ServiceResponse<QuestionCategoryDTO>();

            QuestionCategory categorySaved = _mapper.Map<QuestionCategory>(createCategory);
            _dataContext.QuestionCategories.Add(categorySaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Tạo thành công");

            return serviceResponse;
        }

        //get List chờ có api với user course

        public async Task<ServiceResponse<QuestionCategoryDTO>> getCategoryById( OwnerParameter ownerParameter ,int id)
        {
            var responseServices = new ServiceResponse<QuestionCategoryDTO>();

            var categoryDb = await _dataContext.QuestionCategories.FirstOrDefaultAsync(q => q.IdquestionCategories == id);

            if (categoryDb == null)
            {
                responseServices.updateResponse(404, "Không tồn tại");
            }
            else
            {
                var catDTO = _mapper.Map<QuestionCategoryDTO>(categoryDb);
                catDTO.QuestionBankEntries = (ICollection<QuestionBankEntryDTO>)await _questionServices.getListQuestion(ownerParameter, id);
                responseServices.updateResponse(200, "Ok");
            }
            //categoryDb.QuestionBankEntries = (ICollection<QuestionBankEntry>)await _questionServices.getListQuestion(ownerParameter, id);
            /*            foreach(var qb in categoryDb.QuestionBankEntries)
                        {
                            QuestionBankEntryResponseDTO quesBankResponse = _mapper.Map<QuestionBankEntryResponseDTO>(qb);
                            qb.QuestionVersions = await _questionServices.getQuestionAndAnswerMaxVersion(quesBankResponse).;
                        }*/
           
            return responseServices;
        }
    }
}
