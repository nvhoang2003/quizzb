using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.FormValidator;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Services.AuthServices;
using QuizzBankBE.Services.ListQuestionServices;
using QuizzBankBE.Services.QuestionServices;
using QuizzBankBE.Utility;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]

    // Lam Phan Quyen Sau khi Xu li hoan tat QuestionCategories
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionBankList _questionListServices;
        private readonly IQuestionService _questionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuestionController(IQuestionBankList questionServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration, IQuestionService questionService)
        {
            _questionListServices = questionServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
            _questionService = questionService;
        }


        [HttpPost("AddMultiQuestions")]
        public async Task<ActionResult<ServiceResponse<Boolean>>> addMultiQuestion(List<int> ids)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUESTION").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionListServices.CreateMultiQuestions(ids);

            return Ok(response);
        }

        [HttpGet("GetQuestionById/{id}")]
        public async Task<ActionResult<ServiceResponse<QuestionResponseDTO>>> GetQuestionById(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUESTION").Value;

            if (!CheckPermission.Check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionService.GetQuestionById(id);

            return Ok(response);
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<ActionResult<ServiceResponse<QuestionResponseDTO>>> deleteQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUESTION").Value;
            var deleteQuestion = await _questionService.GetQuestionById(id);

            if (!CheckPermission.Check(userIdLogin, permissionName) || userIdLogin != deleteQuestion.Data?.CreateBy)
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionService.DeleteQuestion(id);
            return Ok(response);
        }
    }
}
