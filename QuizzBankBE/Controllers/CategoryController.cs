using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.CategoryServices;
using QuizzBankBE.Services.QuestionServices;
using System.Security.Claims;
using Newtonsoft.Json;

namespace QuizzBankBE.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public CategoryController(ICategoryServices categoryServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _categoryServices = categoryServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpPost("CreateNewCategory")]
        public async Task<ActionResult<ServiceResponse<QuestionCategoryDTO>>> createNewQuestionn(
       [FromBody] CreateQuestionCategoryDTO createCategoryDTO)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (userIdLogin == null)
            {
                return new StatusCodeResult(401);
            }
            createCategoryDTO.Parent = userIdLogin;
            var response = await _categoryServices.createNewCategory(createCategoryDTO);

            return Ok(response);
        }

        [HttpGet("getCategoriesById/{id}")]
        public async Task<ActionResult<QuestionCategoryDTO>> getCategoriesById(
            [FromQuery] OwnerParameter ownerParameters, int id)
        {
            var response = await _categoryServices.getCategoryById(ownerParameters, id);
            return Ok(response);
        }
    }
}
