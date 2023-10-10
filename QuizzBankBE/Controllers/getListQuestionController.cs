using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
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
    public class getListQuestionController : ControllerBase
    {
        private readonly IQuestionBankList _questionListServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public getListQuestionController(IQuestionBankList questionListServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _questionListServices = questionListServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("getListQuestionBank")]
        public async Task<ActionResult<ServiceResponse<PageList<ListQuestionBank>>>> getListQuestionBank(
            [FromQuery] OwnerParameter ownerParameters, int? categoryId, string? name, string? author, string? questionType, string? tags, DateTime? startDate, DateTime? endDate, bool? isPublic )
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZ_BANK").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionListServices.getListQuestionBank(ownerParameters, userIdLogin, categoryId, name, author,  questionType, tags, startDate, endDate, isPublic);
            
            var metadata = new
            {
                response.Data.TotalCount,
                response.Data.PageSize,
                response.Data.CurrentPage,
                response.Data.TotalPages,
                response.Data.HasNext,
                response.Data.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        [HttpGet("getListQuestion")]
        public async Task<ActionResult<ServiceResponse<PageList<ListQuestion>>>> getListQuestion(
            [FromQuery] OwnerParameter ownerParameters, string? name, string? author, string? questionType, DateTime? startDate, DateTime? endDate)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUESTION").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionListServices.getListQuestion(ownerParameters, userIdLogin, name, author, questionType, startDate, endDate);
            var metadata = new
            {
                response.Data.TotalCount,
                response.Data.PageSize,
                response.Data.CurrentPage,
                response.Data.TotalPages,
                response.Data.HasNext,
                response.Data.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        [HttpPost("AddMultiQuestions")]
        public async Task<ActionResult<ServiceResponse<Boolean>>>addMultiQuestion(List<int> ids)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUESTION").Value;

            if (!CheckPermission.check(userIdLogin, permissionName))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionListServices.createMultiQuestions(ids);

            return Ok(response);
        }

        [HttpDelete("DeleteQuestionBank/{id}")]
        public async Task<ActionResult<Boolean>> deleteQuestionBank(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            var ques = (from q in _dataContext.QuizBanks
                        join qa in _dataContext.QuizbankAnswers on q.Id equals qa.QuizBankId into qaGroup
                        from qa in qaGroup.DefaultIfEmpty()
                        join mq in _dataContext.MatchSubQuestionBanks on q.Id equals mq.QuestionBankId into mqGroup
                        from mq in mqGroup.DefaultIfEmpty()
                        where id == q.Id
                        select new
                        {
                            Question = q,
                            Answer = qa,
                            MatchAnswer = mq
                        }).GroupBy(i => i.Question).Select(g => new GeneralQuestionBankDTO
                        {
                            Question = g.Key,
                            Answers = g.Select(i => i.Answer),
                            MatchAnswers = g.Select(i => i.MatchAnswer)
                        })
                       .FirstOrDefault();

            if (!CheckPermission.isAdmin(userIdLogin) || (!CheckPermission.check(userIdLogin, permissionName) && userIdLogin != ques.Question.AuthorId))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionListServices.deleteQuestionBank(ques);
            if (response.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = response.StatusCode,
                    Title = response.Message
                });
            }
            return Ok(response);
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<ActionResult<Boolean>> deleteQuestion(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:WRITE_QUIZ_BANK").Value;

            var ques = (from q in _dataContext.Questions
                        join qa in _dataContext.QuestionAnswers on q.Id equals qa.QuestionId into qaGroup
                        from qa in qaGroup.DefaultIfEmpty()
                        join mq in _dataContext.MatchSubQuestions on q.Id equals mq.QuestionId into mqGroup
                        from mq in mqGroup.DefaultIfEmpty()
                        where id == q.Id
                        select new
                        {
                            Question = q,
                            Answer = qa,
                            MatchAnswer = mq
                        }).GroupBy(i => i.Question).Select(g => new GeneralQuestionDTO
                        {
                            Question = g.Key,
                            Answers = g.Select(i => i.Answer),
                            MatchAnswers = g.Select(i => i.MatchAnswer)
                        })
                       .FirstOrDefault();

            if (!CheckPermission.isAdmin(userIdLogin) || (!CheckPermission.check(userIdLogin, permissionName) && userIdLogin != ques.Question.AuthorId))
            {
                return new StatusCodeResult(403);
            }

            var response = await _questionListServices.deleteQuestion(ques);
            if (response.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = response.StatusCode,
                    Title = response.Message
                });
            }
            return Ok(response);
        }
    }
}
