using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Services.TagServices;

namespace QuizzBankBE.Services.QuestionServices
{
    public class MultipeChoiceQuestionServicesIpml : IMultipeChoiceQuestionServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public MultipeChoiceQuestionServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public MultipeChoiceQuestionServicesIpml()
        {
        }

        public async Task<ServiceResponse<MultiQuestionDTO>> createNewMultipeQuestion(CreateMultiQuestionDTO createQuestionDTO)
        {
            var serviceResponse = new ServiceResponse<MultiQuestionDTO>();

            Question quesSaved = _mapper.Map<Question>(createQuestionDTO);
            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in createQuestionDTO.Answers)
            {
                createAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo câu hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<MultiQuestionDTO>> getMultipeQuestionById(int Id)
        {
            var serviceResponse = new ServiceResponse<MultiQuestionDTO>();
            var question = await _dataContext.Questions.FirstOrDefaultAsync(c => c.Id == Id && c.QuestionsType == "MultiChoice");

            if (question == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            MultiQuestionDTO questionResponse = _mapper.Map<MultiQuestionDTO>(question);
            var dbAnswers = await _dataContext.QuestionAnswers.Where(c => c.QuestionId.Equals(Id)).ToListAsync();

            questionResponse.Answers = _mapper.Map<List<QuestionAnswerDTO>>(dbAnswers);

            serviceResponse.Data = questionResponse;
            return serviceResponse;
        }

        public QuestionAnswer createAnswer(QuestionAnswerDTO answer, int quizBankId)
        {
            answer.QuestionId = quizBankId;

            QuestionAnswer answerSave = _mapper.Map<QuestionAnswer>(answer);
            _dataContext.QuestionAnswers.Add(answerSave);

            return answerSave;
        }

        public async Task<bool> deleteAnswer(int questionId )
        {
            var dbAnswers = await _dataContext.QuestionAnswers.Where(c => c.QuestionId.Equals(questionId)).ToListAsync();
            foreach (var item in dbAnswers)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QuestionAnswers.UpdateRange(dbAnswers);

            return true;
        }

        public async Task<ServiceResponse<MultiQuestionDTO>> deleteMultipeQuestion(int id)
        {
            var serviceResponse = new ServiceResponse<MultiQuestionDTO>();

            Question quesSaved = _dataContext.Questions.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.Questions.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await deleteAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }
    }
}
