using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Services.RankingServices;
using QuizzBankBE.Services.UserServices;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class RankingController : ControllerBase
    {
        private readonly IRankingServices _rankingServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public RankingController(IRankingServices rankingServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _rankingServices = rankingServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }


        [HttpGet("GetRanking/{id}")]
        public async Task<ActionResult<ServiceResponse<RankingDTO>>> getAllUsers(
          int id)
        {           
            var serviceResponse = await _rankingServices.GetRanking(id);
            
            if(serviceResponse.Status == false)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = serviceResponse.StatusCode,
                    Title = serviceResponse.Message
                });
            }

            return Ok(serviceResponse);
        }
    }
}
