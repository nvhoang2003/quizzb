using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace QuizzBankBE.Services.QuizzResponse
{
    public class QuizResponseServicesImpl : IQuizResponseServices
    {
        public static DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuizResponseServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<AllQuizzResponseDTO>> GetResponseDetail(int accessID)
        {
            var servicesResponse = new ServiceResponse<AllQuizzResponseDTO>();
            var doQuizResponseDTO = new AllQuizzResponseDTO();

            doQuizResponseDTO = (from a in _dataContext.QuizAccesses
                                   join u in _dataContext.Users on a.UserId equals u.Id
                                   join q in _dataContext.Quizzes on a.QuizId equals q.Id
                                   join c in _dataContext.Courses on q.CourseId equals c.Id
                                   where a.Id == accessID
                                   select new AllQuizzResponseDTO
                                   {
                                       quizzAccess = _mapper.Map<QuizAccessDTO>(a),
                                       userDoQuizz = _mapper.Map<UserDTO>(u),
                                       quiz = _mapper.Map<QuizDTO>(q),
                                       course = _mapper.Map<CourseDTO>(c)
                                   }).FirstOrDefault();

            var quizResult = (from qr in _dataContext.QuizResponses
                              join ques in _dataContext.Questions on qr.QuestionId equals ques.Id
                              join qa in _dataContext.QuestionAnswers on ques.Id equals qa.QuestionId into qaGroup
                              from qag in qaGroup.DefaultIfEmpty()
                              join qm in _dataContext.MatchSubQuestions on ques.Id equals qm.QuestionId into qmGroup
                              from qmg in qmGroup.DefaultIfEmpty()
                              where qr.AccessId == accessID
                              select new { qr, ques, qag, qmg }
                             ).AsEnumerable().GroupBy(i => new { i.qr, i.ques }).Distinct().Select(i => new
                             {
                                 QuizzResponse = _mapper.Map<Do1QuizResponseDTO>(i.Key.qr),
                                 Question = _mapper.Map<GeneralQuestionResultDTO>(i.Key.ques),
                                 QuestionAnswer = i.Select(qa => _mapper.Map<QuestionAnswerResultDTO>(qa.qag)).ToList(),
                                 MatchSubQuestion = i.Select(qm => _mapper.Map<MatchSubQuestionResponseDTO>(qm.qmg)).ToList()
                             });

            foreach (var item in quizResult)
            {
                item.QuizzResponse.AnswerToJson = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(item.QuizzResponse?.Answer);
                var arrayQuestionResponse = JArray.Parse(item.QuizzResponse?.Answer);
                List<int> answerChosenId = new List<int>();
                foreach (var oneRes in arrayQuestionResponse)
                {
                    var idToken = oneRes["Id"];
                    if (idToken != null)
                    {
                        int id = idToken.ToObject<int>();
                        answerChosenId.Add(id);
                    }
                }
                foreach(var answer in item.QuestionAnswer)
                {
                    answer.isChosen = answerChosenId.Contains(answer.Id) ? true : false;
                }
                doQuizResponseDTO.questionReults.Add(item);
                doQuizResponseDTO.totalPoint += item.QuizzResponse.Mark;
            }

            doQuizResponseDTO.status = doQuizResponseDTO.totalPoint >= doQuizResponseDTO.quiz.PointToPass ? "Pass" : "Failed";

            servicesResponse.Data = doQuizResponseDTO;
            servicesResponse.Message = "OK";

            return servicesResponse;
        }

        public async Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> getListResponseForDoQuiz(OwnerParameter ownerParameters, int userIdLogin, int? quizId, int? courseId, DateTime? timeStart, DateTime? timeEnd)
        {
            var serviceResponse = new ServiceResponse<PageList<AllQuizzResponseDTO>>();
            var doQuizResponseDTO = new List<AllQuizzResponseDTO>();

            doQuizResponseDTO = await (from a in _dataContext.QuizAccesses
                                       join u in _dataContext.Users on a.UserId equals u.Id
                                       join q in _dataContext.Quizzes on a.QuizId equals q.Id
                                       join c in _dataContext.Courses on q.CourseId equals c.Id
                                       where a.UserId == userIdLogin && (quizId == null || a.QuizId == quizId) && (courseId == null || c.Id == courseId) && (timeStart == null || timeStart == null || (c.CreateDate >= timeStart && c.CreateDate <= timeStart))
                                       let totalPoint = a.QuizResponses.Sum(x => x.Mark)
                                       select new AllQuizzResponseDTO
                                       {
                                           quizzAccess = _mapper.Map<QuizAccessDTO>(a),
                                           userDoQuizz = _mapper.Map<UserDTO>(u),
                                           quiz = _mapper.Map<QuizDTO>(q),
                                           course = _mapper.Map<CourseDTO>(c),
                                           totalPoint = totalPoint,
                                           status = totalPoint > q.PointToPass ? "Pass" : "Failed"
                                       }).ToListAsync();


            serviceResponse.Data = PageList<AllQuizzResponseDTO>.ToPageList(
                doQuizResponseDTO.AsEnumerable<AllQuizzResponseDTO>(),
                ownerParameters.pageIndex,
                ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> getListResponseForWriteQuiz(OwnerParameter ownerParameters, int? quizId, int? courseId, string? name)
        {
            var serviceResponse = new ServiceResponse<PageList<AllQuizzResponseDTO>>();
            var doQuizResponseDTO = new List<AllQuizzResponseDTO>();

            doQuizResponseDTO = await(from a in _dataContext.QuizAccesses
                                      join u in _dataContext.Users on a.UserId equals u.Id
                                      join q in _dataContext.Quizzes on a.QuizId equals q.Id
                                      join c in _dataContext.Courses on q.CourseId equals c.Id
                                      let fullName = u.FirstName + " " + u.LastName
                                      where (name == null || fullName.Contains(name)) && (quizId == null || a.QuizId == quizId) && (courseId == null || c.Id == courseId)
                                      let totalPoint = a.QuizResponses.Sum(x => x.Mark)
                                      select new AllQuizzResponseDTO
                                      {
                                          quizzAccess = _mapper.Map<QuizAccessDTO>(a),
                                          userDoQuizz = _mapper.Map<UserDTO>(u),
                                          quiz = _mapper.Map<QuizDTO>(q),
                                          course = _mapper.Map<CourseDTO>(c),
                                          totalPoint = totalPoint,
                                          status = totalPoint > q.PointToPass ? "Pass" : "Failed"
                                      }).ToListAsync();


            serviceResponse.Data = PageList<AllQuizzResponseDTO>.ToPageList(
                doQuizResponseDTO.AsEnumerable<AllQuizzResponseDTO>(),
                ownerParameters.pageIndex,
                ownerParameters.pageSize);
            return serviceResponse;
        }
    }
}
