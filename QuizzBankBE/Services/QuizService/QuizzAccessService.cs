using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using System.Linq;

namespace QuizzBankBE.Services.QuizService
{
    public class QuizzAccessService : IQuizzAccessService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuizzAccessService(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public QuizzAccessService()
        {
        }

        public async Task<ServiceResponse<QuizAccessDTO>> CreateQuizzAccess(CreateQuizAccessDTO createAccessDTO)
        {
            var serviceResponse = new ServiceResponse<QuizAccessDTO>();
            createAccessDTO.Status = "Wait";

            QuizAccess quizSaved = _mapper.Map<QuizAccess>(createAccessDTO);

            _dataContext.QuizAccesses.Add(quizSaved);
            await _dataContext.SaveChangesAsync();
            serviceResponse.Message = "Tạo thành công";
            serviceResponse.Data = _mapper.Map<QuizAccessDTO>(quizSaved);
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizAccessDTO>> UpdateStatus(CreateQuizAccessDTO updateStatusDTO, int id)
        {
            var serviceResponse = new ServiceResponse<QuizAccessDTO>();

            var quesToUpdate = _dataContext.QuizAccesses.FirstOrDefault(c => c.Id == id);
            _mapper.Map(updateStatusDTO, quesToUpdate);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật status thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<QuizAccessDTO>>> getListQuizzAccess(OwnerParameter ownerParameters, int? courseId, int? studentId)
        {
            var serviceResponse = new ServiceResponse<PageList<QuizAccessDTO>>();

            var dbQuizAccess = _dataContext.QuizAccesses.
                    Include(q => q.Quiz).
                    ThenInclude(qa => qa.Course).
                    Where(q => (courseId == null || q.Quiz.CourseId == courseId) && (studentId == null || q.UserId == studentId)).ToList();

            var quizAccessResponse = _mapper.Map<List<QuizAccessDTO>>(dbQuizAccess);
            serviceResponse.Data = PageList<QuizAccessDTO>.ToPageList(
            quizAccessResponse.AsEnumerable<QuizAccessDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            serviceResponse.Status = true;

            return serviceResponse;
        }
    }
}
