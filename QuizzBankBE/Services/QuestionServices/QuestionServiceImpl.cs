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

            var ques = await _dataContext.Questions
               .Include(i => i.QuestionAnswers)
               .ThenInclude(i => i.SystemFile)
               .Include(i => i.MatchSubQuestions)
               .ThenInclude(i => i.SystemFile)
               .Include(i => i.SystemFile)
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync();

            if (ques == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            quesResponse = _mapper.Map<QuestionResponseDTO>(ques);

            if (quesResponse.SystemFile?.NameFile != null)
            {
                quesResponse.ImageUrl = _configuration["LinkShowImage"] + quesResponse.SystemFile.NameFile;
            }


            if (quesResponse.QuestionsType == "Match")
            {
                foreach (var item in quesResponse.MatchSubQuestions)
                {
                    if (item.SystemFile?.NameFile != null)
                    {
                        item.ImageUrl = _configuration["LinkShowImage"] + item.SystemFile.NameFile;
                    }
                }
            }
            else
            {
                foreach (var item in quesResponse.QuestionAnswers)
                {
                    if (item.SystemFile?.NameFile != null)
                    {
                        item.ImageUrl = _configuration["LinkShowImage"] + item.SystemFile.NameFile;
                    }
                }
            }

            serviceResponse.Data = quesResponse;

            return serviceResponse;
        }
    }
}
