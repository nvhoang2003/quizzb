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

        public async Task<ServiceResponse<QuestionCategoryDTO>> updateCategory(int id,CreateQuestionCategoryDTO createCategory)
        {
            var serviceResponse = new ServiceResponse<QuestionCategoryDTO>();
            var categoryDb = _dataContext.QuestionCategories.FirstOrDefaultAsync(q => q.IdquestionCategories == id).Result;

            if(categoryDb == null)
            {
                serviceResponse.updateResponse(404, "Không Tồn Tại");
            }
            else
            {
                categoryDb.Name = createCategory.Name;
                _dataContext.QuestionCategories.Update(categoryDb);
                await _dataContext.SaveChangesAsync();
               
                serviceResponse.updateResponse(200, "Sửa thành công");
            }
           

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionCategoryDTO>> deleteCategory(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionCategoryDTO>();
            var categoryDb = _dataContext.QuestionCategories.FirstOrDefaultAsync(q => q.IdquestionCategories == id).Result;

            if (categoryDb == null)
            {
                serviceResponse.updateResponse(404, "Không Tồn Tại");
            }
            else
            {
                categoryDb.IsDeleted = 1;
                _dataContext.QuestionCategories.Update(categoryDb);
                await _dataContext.SaveChangesAsync();

                serviceResponse.updateResponse(200, "Xóa thành công");
            }


            return serviceResponse;
        }
    }
}
