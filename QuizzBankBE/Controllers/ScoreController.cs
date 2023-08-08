﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
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
    public class ScoreController : ControllerBase
    {
        private readonly IScoreServicesImpl _scoreServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ScoreController(IScoreServicesImpl scoreServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _scoreServices = scoreServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("{quizID}/doMatchQuestion")]
        public async Task<ActionResult<ServiceResponse<float>>> DoMatchQuestion(DoMatchingDTO doQuestionDTO, int quizID)
        {
            var userIdLogin = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var hasQuizAccess = await HasQuizAccess(doQuestionDTO.QuizAccessID, userIdLogin, quizID);

            if (!hasQuizAccess)
            {
                return new StatusCodeResult(403);
            }

            var response = await _scoreServices.doQuestion(doQuestionDTO);

            return Ok(response);
        }

        private async Task<bool> HasQuizAccess(int quizAcessID, int userID, int quizID)
        {
            var quizAccess = await _dataContext.QuizAccesses.FirstOrDefaultAsync(e => e.Id == quizAcessID && e.UserId == userID && e.QuizId == quizID);

            if (quizAccess == null)
            {
                return false;
            }

            return true;
        }
    }
}
