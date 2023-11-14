using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using Newtonsoft.Json;


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

        public async Task<ServiceResponse<QuizResponseDetailDTO>> GetResponseDetail(int accessID)
        {
            var servicesResponse = new ServiceResponse<QuizResponseDetailDTO>();
            var doQuizResponseDTO = new QuizResponseDetailDTO();

            var quizID = await _dataContext.QuizAccesses.Where(q => q.Id == accessID).Select(q => q.QuizId).FirstOrDefaultAsync();

            var dbQuestion = await _dataContext.Questions.
                Include(q => q.SystemFile).
                Include(q => q.MatchSubQuestions).
                Include(q => q.QuestionAnswers).
                Include(q => q.QuizQuestions.Where(q => q.QuizzId == quizID)).
                    ThenInclude(qq => qq.Quizz).
                Include(q => q.QuizResponses.Where(qr => qr.AccessId == accessID)).
                    ThenInclude(q => q.Access).
                        ThenInclude(q => q.Quiz).
                            ThenInclude(q => q.Course).
                Include(q => q.QuizResponses.Where(qr => qr.AccessId == accessID)).
                    ThenInclude(q => q.Access).
                        ThenInclude(q => q.User).
                Where(q => q.QuizQuestions.Any(qq => qq.QuizzId == quizID)).
                Distinct().
                ToListAsync();


            if (dbQuestion.Count == 0)
            {
                servicesResponse.updateResponse(400, "không tồn tại");
                return servicesResponse;
            }

            var access = dbQuestion.Where(q => q.QuizResponses.Count > 0)?.FirstOrDefault()?.QuizResponses.FirstOrDefault()?.Access;
            var quizz = dbQuestion?.FirstOrDefault()?.QuizQuestions.FirstOrDefault()?.Quizz;

            doQuizResponseDTO.UserName = access?.User?.FirstName + " " + access?.User?.LastName;
            doQuizResponseDTO.CourseName = access?.Quiz?.Course?.FullName;
            doQuizResponseDTO.QuizName = quizz?.Name;
            doQuizResponseDTO.PointToPass = quizz?.PointToPass;
            doQuizResponseDTO.MaxPoint = quizz?.MaxPoint;
            doQuizResponseDTO.isPublic = quizz?.IsPublic;
            doQuizResponseDTO.TotalPoint = 0;

            if(access?.TimeEndQuiz != null && access?.TimeStartQuiz != null)
            {
                TimeSpan diff = access.TimeEndQuiz.Value.Subtract(access.TimeStartQuiz.Value);
                doQuizResponseDTO.DiffTime = diff.ToString("mm\\:ss");
            }
            else
            {
                doQuizResponseDTO.DiffTime = "--:--";
            }
            

            foreach (var item in dbQuestion)
            {
                QuestionResultDTO questionResult = new QuestionResultDTO();

                questionResult.question = _mapper.Map<QuestionResponseDTO>(item);

                if (item.SystemFile?.NameFile != null)
                {
                    questionResult.question.ImageUrl = _configuration["LinkShowImage"] + item.SystemFile.NameFile;
                }

                switch (item.QuestionsType)
                {
                    case "MultiChoice":
                        questionResult.IdAnswerChoosen = JsonConvert.DeserializeObject<List<int>>(item.QuizResponses.FirstOrDefault()?.Answer == null ? "" : item.QuizResponses.FirstOrDefault()?.Answer);
                        break;

                    case "Match":
                        questionResult.MatchSubQuestionChoosen = JsonConvert.DeserializeObject<List<MatchSubQuestionChoosenDTO>>(item.QuizResponses.FirstOrDefault()?.Answer == null ? "": item.QuizResponses.FirstOrDefault()?.Answer);
                        break;

                    case "ShortAnswer":
                        questionResult.ShortAnswerChoosen = JsonConvert.DeserializeObject<string>(item.QuizResponses.FirstOrDefault()?.Answer == null ? "" : item.QuizResponses.FirstOrDefault()?.Answer);
                        break;

                    case "TrueFalse":
                        questionResult.IdAnswerChoosen = JsonConvert.DeserializeObject<List<int>>(item.QuizResponses.FirstOrDefault()?.Answer == null ? "" : item.QuizResponses.FirstOrDefault()?.Answer);
                        break;

                    default:
                        break;
                }
                questionResult.Mark = item.QuizResponses.FirstOrDefault()?.Mark;
                questionResult.Status = item.QuizResponses.FirstOrDefault()?.Status;

                doQuizResponseDTO.TotalPoint += item.QuizResponses.FirstOrDefault()?.Mark == null ? 0 : item.QuizResponses.FirstOrDefault()?.Mark ;
                doQuizResponseDTO.questionReults.Add(questionResult);
            }

            doQuizResponseDTO.status = doQuizResponseDTO.TotalPoint >= doQuizResponseDTO.PointToPass ? "Pass" : "Failed";

            servicesResponse.Data = doQuizResponseDTO;
            servicesResponse.Message = "OK";

            return servicesResponse;
        }

        public async Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> GetListResponseForDoQuiz(OwnerParameter ownerParameters, int userIdLogin, int? quizId, int? courseId, DateTime? timeStart, DateTime? timeEnd)
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

        public async Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> GetListResponseForWriteQuiz(OwnerParameter ownerParameters, int? quizId, int? courseId, string? name)
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
