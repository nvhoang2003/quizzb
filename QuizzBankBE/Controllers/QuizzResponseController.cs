using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Services.ScoreServices;
using QuizzBankBE.Utility;
using System.Security.Claims;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class QuizzResponseController : ControllerBase
    {
        private readonly IScoreServicesImpl _scoreServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public QuizzResponseController(IScoreServicesImpl scoreServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _scoreServices = scoreServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("getQuizzResponse/{id}")]
        public async Task<ActionResult<AllQuizzResponseDTO>> GetQuizzResponse(int id)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var permissionName = _configuration.GetSection("Permission:READ_QUIZZ_RESPONSE").Value;
            var accessQuizz = _dataContext.QuizAccesses.Where(q => q.Id == id).FirstOrDefaultAsync().Result;
            if (accessQuizz == null)
            {
                return new StatusCodeResult(404);
            }

            if (!CheckPermission.check(userIdLogin, permissionName) && userIdLogin != accessQuizz.UserId)
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.GetScore(id);
            return Ok(response);
        }
    }
}
