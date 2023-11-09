using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Utility;
using System.Linq;
using System.Security.Claims;

namespace QuizzBankBE.Services.QuizService
{
    public class QuizzAccessService : IQuizzAccessService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizzAccessService(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public QuizzAccessService()
        {
        }

        public async Task<ServiceResponse<QuizAccessDTO>> CreateQuizzAccess(CreateQuizAccessDTO createAccessDTO)
        {
            var serviceResponse = new ServiceResponse<QuizAccessDTO>();

            if(_dataContext.QuizAccesses.Any(q => q.UserId == createAccessDTO.UserId && q.QuizId == createAccessDTO.QuizId))
            {
                serviceResponse.Message = "Học sinh đã được chọn làm đề này. Vui lòng không thao tác lại";
                serviceResponse.StatusCode = 400;
                serviceResponse.Status = false;
                return serviceResponse;
            }

            var isPermiss = await CheckMutationQuizzAccessPermission(createAccessDTO);

            if(isPermiss.Data == false)
            {
                serviceResponse.updateResponse(403, "Bạn không có quyền!");
                return serviceResponse;
            }

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

            var isPermiss = await CheckMutationQuizzAccessPermission(updateStatusDTO);

            if (isPermiss.Data == false)
            {
                serviceResponse.updateResponse(403, "Bạn không có quyền!");
                return serviceResponse;
            }

            var quesToUpdate = _dataContext.QuizAccesses.FirstOrDefault(c => c.Id == id);

            if(quesToUpdate.Status != "Wait")
            {
                serviceResponse.updateResponse(400, "Bạn không thể cập nhật vì học sinh đã làm xong bài kiểm tra!");
                return serviceResponse;
            }

            _mapper.Map(updateStatusDTO, quesToUpdate);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật status thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<QuizAccessDTO>>> GetListQuizzAccess(OwnerParameter ownerParameters, int? courseId, string? studentName, string? status, bool? isPublic)
        {
            var serviceResponse = new ServiceResponse<PageList<QuizAccessDTO>>();

            var dbQuizAccess = _dataContext.QuizAccesses.
                    Include(q => q.Quiz).
                    ThenInclude(qa => qa.Course).
                    Include(q => q.User).
                    Where(q => (courseId == null || q.Quiz.CourseId == courseId) && (studentName == null || (q.User.FirstName + " " + q.User.LastName).Contains(studentName)) && (status == null || q.Status == status) && (isPublic == null || q.Quiz.IsPublic == Convert.ToSByte(isPublic))).ToList();

            var quizAccessResponse = _mapper.Map<List<QuizAccessDTO>>(dbQuizAccess);

            serviceResponse.Data = PageList<QuizAccessDTO>.ToPageList(
            quizAccessResponse.AsEnumerable<QuizAccessDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            serviceResponse.Status = true;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Boolean>> CheckMutationQuizzAccessPermission(CreateQuizAccessDTO createAccessDTO)
        {
            var serviceResponse = new ServiceResponse<Boolean>();

            var userIdLogin = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var isPublicQuiz = await _dataContext.Quizzes.Where(q => q.Id == createAccessDTO.QuizId).Select(q => q.IsPublic).FirstOrDefaultAsync();
            
            if(isPublicQuiz == 0)
            {
                var permissionName = _configuration.GetSection("Permission:WRITE_LIST_STUDENT_DO_QUIZZ").Value;

                if (!CheckPermission.Check(userIdLogin, permissionName))
                {
                    serviceResponse.Data = false;
                    return serviceResponse;
                }
            }
            else
            {
                if(userIdLogin != createAccessDTO.UserId)
                {
                    serviceResponse.Data = false;
                    return serviceResponse;
                }
            }

            serviceResponse.Data = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizAccessDTO>> DeleteQuizAccess(int id)
        {
            var serviceResponse = new ServiceResponse<QuizAccessDTO>();

            var quizAccess = await _dataContext.QuizAccesses.Where(qa => qa.Id == id).FirstOrDefaultAsync();

            if(quizAccess == null)
            {
                serviceResponse.Status = false;
                serviceResponse.updateResponse(404, "Học sinh này đã bị đưa ra khỏi danh sách làm đề thi");
                return serviceResponse;
            }

            if (quizAccess.Status != "Wait")
            {
                serviceResponse.updateResponse(400, "Bạn không thể cập nhật vì học sinh đã làm xong bài kiểm tra!");
                return serviceResponse;
            }

            quizAccess.IsDeleted = 1;
            _dataContext.QuizAccesses.Update(quizAccess);
            _dataContext.SaveChangesAsync();

            serviceResponse.Status = true;
            serviceResponse.updateResponse(200, "Xóa thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<QuizAccessDTO>>> GetListExam(int? courseId, int? studentId, string? status, bool? isPublic)
        {
            var serviceResponse = new ServiceResponse<List<QuizAccessDTO>>();

            var dbQuizAccess = _dataContext.QuizAccesses.
                    Include(q => q.Quiz).
                    ThenInclude(qa => qa.Course).
                    Include(q => q.User).
                    Where(q => (courseId == null || q.Quiz.CourseId == courseId) && (studentId == null || q.UserId == studentId) && (status == null || q.Status == status) && (isPublic == null || q.Quiz.IsPublic == Convert.ToSByte(isPublic))).ToList();

            var quizAccessResponse = _mapper.Map<List<QuizAccessDTO>>(dbQuizAccess);

            serviceResponse.Data = quizAccessResponse;
            serviceResponse.Status = true;

            return serviceResponse;
        }
    }
}
