using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public class ShortAnswerQuestionServicesIpml : IShortAnswerQuestionServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public ShortAnswerQuestionServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public ShortAnswerQuestionServicesIpml()
        {
        }

        public async Task<ServiceResponse<ShortAnswerQuestionDTO>> CreateShortAnswerQuestion(CreateShortAnswerQuestionDTO createQuestionDTO)
        {
            var serviceResponse = new ServiceResponse<ShortAnswerQuestionDTO>();

            Question quesSaved = _mapper.Map<Question>(createQuestionDTO);
            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in createQuestionDTO.Answers)
            {
                CreateAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<ShortAnswerQuestionDTO>> DeleteShortAnswerQuestion(int id)
        {
            var serviceResponse = new ServiceResponse<ShortAnswerQuestionDTO>();

            Question quesSaved = _dataContext.Questions.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.Questions.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await DeleteAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<ShortAnswerQuestionDTO>> GetShortAnswerQuestionById(int id)
        {
            var serviceResponse = new ServiceResponse<ShortAnswerQuestionDTO>();
            var question = await _dataContext.Questions.FirstOrDefaultAsync(c => c.Id == id && c.QuestionsType == "ShortAnswer");

            if (question == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            ShortAnswerQuestionDTO questionResponse = _mapper.Map<ShortAnswerQuestionDTO>(question);
            var dbAnswers = await _dataContext.QuestionAnswers.ToListAsync();

            questionResponse.Answers = dbAnswers.Select(u => _mapper.Map<QuestionAnswerDTO>(u)).Where(c => c.QuestionId.Equals(id)).ToList();

            serviceResponse.Data = questionResponse;
            return serviceResponse;
        }

        public QuestionAnswer CreateAnswer(QuestionAnswerDTO answer, int quizBankId)
        {
            answer.QuestionId = quizBankId;

            QuestionAnswer answerSave = _mapper.Map<QuestionAnswer>(answer);
            _dataContext.QuestionAnswers.Add(answerSave);

            return answerSave;
        }

        public async Task<bool> DeleteAnswer(int quizBankId)
        {
            var dbAnswers = await _dataContext.QuestionAnswers.Where(c => c.QuestionId.Equals(quizBankId)).ToListAsync();
            foreach (var item in dbAnswers)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QuestionAnswers.UpdateRange(dbAnswers);

            return true;
        }
    }
}
