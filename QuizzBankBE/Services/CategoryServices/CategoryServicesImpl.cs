﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.CategoryServices
{
    public class CategoryServicesImpl : ICategoryServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        //private readonly IQuestionServices _questionServices;

        public CategoryServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public CategoryServicesImpl()
        {
        }
        public async Task<ServiceResponse<PageList<CategoryDTO>>> GetAllCategory(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<CategoryDTO>>();
            var dbCategory = await _dataContext.Categories.ToListAsync();

            var categoryDTO  = dbCategory.Select(u => _mapper.Map<CategoryDTO>(u)).ToList();
            
            serviceResponse.Data = PageList<CategoryDTO>.ToPageList(
            categoryDTO.AsEnumerable<CategoryDTO>(),    
            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<CategoryDTO>> GetCategoryByID(int categoryID)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            var dbCategory = await _dataContext.Categories.ToListAsync();

            var categoryDTO = dbCategory.Select(u => _mapper.Map<CategoryDTO>(u)).Where(c => c.Id == categoryID).FirstOrDefault();
            if (categoryDTO == null)
            {
                serviceResponse.updateResponse(400, "không tồn tại");
                return serviceResponse;
            }
            
            serviceResponse.Data = categoryDTO;
            return serviceResponse;
        }

        public async Task<ServiceResponse<CategoryDTO>> CreateNewCategory(CreateCategoryDTO createCategory)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();

            Category categorySaved = _mapper.Map<Category>(createCategory);
            _dataContext.Categories.Add(categorySaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Tạo thành công");

            return serviceResponse;
        }

        //get List chờ có api với user course

        public async Task<ServiceResponse<CategoryDTO>> UpdateCategory(int id, CreateCategoryDTO createCategory)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            var categoryDb = _dataContext.Categories.FirstOrDefaultAsync(q => q.Id == id).Result;

            if (categoryDb == null)
            {
                serviceResponse.updateResponse(404, "Không Tồn Tại");
            }
            else
            {
                categoryDb.Name = createCategory.Name;
                categoryDb.Description = createCategory.Description;
                _dataContext.Categories.Update(categoryDb);
                await _dataContext.SaveChangesAsync();

                serviceResponse.updateResponse(200, "Sửa thành công");
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<CategoryDTO>> DeleteCategory(int id)
        {
            var serviceResponse = new ServiceResponse<CategoryDTO>();
            var categoryDb = _dataContext.Categories.FirstOrDefaultAsync(q => q.Id== id).Result;
            
            var tag = await _dataContext.Tags.ToListAsync();
            var tags = tag.Select(u => _mapper.Map<TagDTO>(u)).Where(t => t.CategoryId == id).ToList();

            if (categoryDb == null )
            {
                serviceResponse.updateResponse(404, "Không Tồn Tại");
            }
            else
            {
                if (tags?.Count() != 0) {
                    serviceResponse.updateResponse(400,"Không thể xóa ");
                }
                else
                {
                    categoryDb.IsDeleted = 1;
                    _dataContext.Categories.Update(categoryDb);
                    await _dataContext.SaveChangesAsync();

                    serviceResponse.updateResponse(200, "Xóa thành công");
                }
                
            }


            return serviceResponse;
        }
    }
}
