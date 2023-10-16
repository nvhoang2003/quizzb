using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public class QuestionServiceImpl : IQuestionService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuestionServiceImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _configuration = configuration;
            _jwtProvider = jwtProvider;
        }

        public async Task<ServiceResponse<QuestionResponseDTO>> DeleteQuestion(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

            var ques = await _dataContext.Questions
                 .Include(i => i.QuestionAnswers)
                 .Include(i => i.MatchSubQuestions)
                 .Where(c => c.Id == id)
                 .FirstOrDefaultAsync();

            ques.IsDeleted = 1;
            if (ques.QuestionAnswers != null)
            {
                foreach (var it in ques.QuestionAnswers)
                {
                    it.IsDeleted = 1;
                }
            }
            if (ques.MatchSubQuestions != null)
            {
                foreach (var it in ques.MatchSubQuestions)
                {
                    it.IsDeleted = 1;
                }
            }     

            serviceResponse.updateResponse(200, "Xoá câu hỏi thành công");

            await _dataContext.SaveChangesAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionResponseDTO>> GetQuestionById(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

            var quesResponse = new QuestionResponseDTO();

            var ques = (from q in _dataContext.Questions
                        join qa in _dataContext.QuestionAnswers on q.Id equals qa.QuestionId into qaGroup
                        from qa in qaGroup.DefaultIfEmpty()
                        join mq in _dataContext.MatchSubQuestions on q.Id equals mq.QuestionId into mqGroup
                        from mq in mqGroup.DefaultIfEmpty()
                        where q.Id == id
                        select new
                        {
                            Question = q,
                            Answer = qa,
                            MatchAnswer = mq
                        }).GroupBy(i => i.Question).Select(g => new
                        {
                            Question = g.Key,
                            Answers = g.Select(i => i.Answer),
                            MatchAnswers = g.Select(i => i.MatchAnswer),
                        })
                        .FirstOrDefault();

            if (ques == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            quesResponse = _mapper.Map<QuestionResponseDTO>(ques.Question);
            quesResponse.QuestionAnswers = _mapper.Map<List<QuestionAnswerResponseDTO>>(ques.Answers.Distinct());
            quesResponse.MatchSubQuestions = _mapper.Map<List<MatchSubQuestionResponseDTO>>(ques.MatchAnswers.Distinct());

            serviceResponse.Data = quesResponse;

            return serviceResponse;
        }
    }
}
