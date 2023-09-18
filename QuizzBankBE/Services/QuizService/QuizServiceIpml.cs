using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using System.Linq;

namespace QuizzBankBE.Services.QuizService
{
    public class QuizServiceIpml : IQuizService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuizServiceIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public QuizServiceIpml()
        {
        }

        public async Task<ServiceResponse<QuizResponseDTO>> createNewQuiz(CreateQuizDTO createQuizDTO)
        {
            var serviceResponse = new ServiceResponse<QuizResponseDTO>();
            var quizSaved = _mapper.Map<Quiz>(createQuizDTO);

            _dataContext.Quizzes.Add(quizSaved);
            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<QuizDTO>>> getAllQuiz(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<QuizDTO>>();
            var dbQuiz = await _dataContext.Quizzes.ToListAsync();
            var quizDTO = dbQuiz.Select(u => _mapper.Map<QuizDTO>(u)).ToList();

            foreach(var item in quizDTO)
            {
                item.IsValid = item.MaxPoint > item.PointToPass ? true : false;
            }

            serviceResponse.Data = PageList<QuizDTO>.ToPageList(
            quizDTO.AsEnumerable<QuizDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizResponseDTO>> updateQuizz(CreateQuizDTO updateQuizDTO, int id)
        {
            var serviceResponse = new ServiceResponse<QuizResponseDTO>();
            var dbQuiz = await _dataContext.Quizzes.FirstOrDefaultAsync(q => q.Id == id);

            if (dbQuiz == null)
            {
                serviceResponse.updateResponse(400, "quizz không tồn tại");
                return serviceResponse;
            }

            var quizSaved = _mapper.Map(updateQuizDTO, dbQuiz);

            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Update thành công";
            serviceResponse.Data = _mapper.Map<QuizResponseDTO>(quizSaved);

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizDTO>> deleteQuizz(int id)
        {
            var serviceResponse = new ServiceResponse<QuizDTO>();
            var dbQuiz = await _dataContext.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
            if (dbQuiz == null)
            {
                serviceResponse.updateResponse(400, "Quizz không tồn tại");
                return serviceResponse;
            }

            dbQuiz.IsDeleted = 1;
            _dataContext.Quizzes.Update(dbQuiz);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Delete thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizQuestionDTO>> addQuestionIntoQuiz (CreateQuizQuestionDTO createQuizQuestionDTO)
        {
            var serviceResponse = new ServiceResponse<QuizQuestionDTO>();
            List<QuizQuestion> quizQuestionsSaved = new List<QuizQuestion>();
            QuizQuestion oneQuizQuestionSaved = new QuizQuestion();
            int? quizId = createQuizQuestionDTO.QuizzId;

            foreach (var item in createQuizQuestionDTO.questionAddeds)
            {
                oneQuizQuestionSaved.QuizzId = quizId;
                oneQuizQuestionSaved.QuestionId = item.QuestionId;
                oneQuizQuestionSaved.Point = item.Point;

                quizQuestionsSaved.Add(oneQuizQuestionSaved);
            }

            Quiz dbQuiz = await _dataContext.Quizzes.FirstOrDefaultAsync(q => q.Id.Equals(quizId));
            dbQuiz.MaxPoint = createQuizQuestionDTO.questionAddeds.Select(c => c.Point).Sum();
            _dataContext.Quizzes.Update(dbQuiz);
            _dataContext.QuizQuestions.AddRange(quizQuestionsSaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Tạo thành công";
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizDetailResponseDTO>> getQuizById(int id)
        {
            ServiceResponse<QuizDetailResponseDTO> serviceResponse = new ServiceResponse<QuizDetailResponseDTO>();

            var quizDbDetail = (from qi in _dataContext.Quizzes
                                join qq in _dataContext.QuizQuestions on qi.Id equals qq.QuizzId
                                join qe in _dataContext.Questions on qq.QuestionId equals qe.Id
                                where qi.Id == id
                                select new { qi,qe }
                                ).ToList();

            QuizDetailResponseDTO quizDetail = _mapper.Map<QuizDetailResponseDTO>(quizDbDetail.First().qi);

            if (quizDetail == null)
            {
                serviceResponse.updateResponse(400, "không tồn tại");
                return serviceResponse;
            }

            quizDetail.listQuestion = _mapper.Map<List<ListQuestion>>(quizDbDetail.Select(c => c.qe).ToList());

            serviceResponse.Data = quizDetail;
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuizResponseForTest>> showQuizForTest(int id)
        {
            ServiceResponse<QuizResponseForTest> serviceResponse = new ServiceResponse<QuizResponseForTest>();
            QuizResponseForTest quizResponseForTest = new QuizResponseForTest();
            
            var dbQuiz = await _dataContext.Quizzes.ToListAsync();

            quizResponseForTest.quiz = dbQuiz.Where(c => c.Id == id).Select(u => _mapper.Map<QuizDTO>(u)).FirstOrDefault();

            var quizResult = (from qi in _dataContext.Quizzes
                              join qq in _dataContext.QuizQuestions on qi.Id equals qq.QuizzId
                              join ques in _dataContext.Questions on qq.QuestionId equals ques.Id
                              join qa in _dataContext.QuestionAnswers on ques.Id equals qa.QuestionId into qaGroup
                              from qag in qaGroup.DefaultIfEmpty()
                              join qm in _dataContext.MatchSubQuestions on ques.Id equals qm.QuestionId into qmGroup
                              from qmg in qmGroup.DefaultIfEmpty()
                              where qi.Id == id
                              select new { qi, ques, qag, qmg }
                          ).AsEnumerable().GroupBy(i => new { i.qi, i.ques }).Distinct().Select(i => new
                          {
                              Question = _mapper.Map<GeneralQuestionResultDTO>(i.Key.ques),
                              QuestionAnswer = i.Select(qa => _mapper.Map<QuestionAnswerResultDTO>(qa.qag)).ToList(),
                              MatchSubQuestion = i.Select(qm => _mapper.Map<MatchSubQuestionResponseDTO>(qm.qmg)).ToList()
                          });

            if (quizResult == null)
            {
                serviceResponse.updateResponse(400, "không tồn tại");
                return serviceResponse;
            }

            foreach(var item in quizResult)
            {
                quizResponseForTest.questionReults.Add(item);
            }

            serviceResponse.Data = quizResponseForTest;

            return serviceResponse;
        }
    }
}
