using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.Services.ListQuestionServices;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class GetFixDataController : ControllerBase
    {
        static List<QuestionTypeDTO> questionList = new List<QuestionTypeDTO>
{
            new QuestionTypeDTO { name = "MultiChoice", value = "MultiChoice" },
            new QuestionTypeDTO { name = "TrueFalse", value = "TrueFalse" },
            new QuestionTypeDTO { name = "Match", value = "Match" },
            new QuestionTypeDTO { name = "ShortAnswer", value = "ShortAnswer" },
            new QuestionTypeDTO { name = "Numerical", value = "Numerical" },
            new QuestionTypeDTO { name = "DragAndDropIntoText", value = "DragAndDropIntoText" },
        };

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;


        public GetFixDataController(IQuestionBankList questionListServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("getListQuestionBank")]
        public async Task<ActionResult<ServiceResponse<List<QuestionTypeDTO>>>> getListQuestionType()
        {
            var response = new ServiceResponse<List<QuestionTypeDTO>>();

            response.Data = questionList;

            return Ok(response);
        }

    }
}
