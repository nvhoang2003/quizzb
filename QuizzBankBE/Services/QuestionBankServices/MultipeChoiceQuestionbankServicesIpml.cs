using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Services.TagServices;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class MultipeChoiceQuestionbankServicesIpml : IMultipeChoiceQuizBankServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public MultipeChoiceQuestionbankServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public MultipeChoiceQuestionbankServicesIpml()
        {
        }

        public async Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> createNewMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO createQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMultipeChoiceResponseDTO>();

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in createQuestionBankDTO.Answers)
            {
                createAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo câu hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> getMultipeQuestionBankById(int Id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMultipeChoiceResponseDTO>();
            var quizBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == Id && c.QuestionsType == "MultiChoice");

            if (quizBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            QuestionBankMultipeChoiceResponseDTO quizBankResponse = _mapper.Map<QuestionBankMultipeChoiceResponseDTO>(quizBank);
            var dbAnswers = await _dataContext.QuizbankAnswers.Where(c => c.QuizBankId.Equals(Id)).ToListAsync();

            quizBankResponse.Answers = _mapper.Map<List<QuestionBankAnswerDTO>>(dbAnswers);
            quizBankResponse.addTags(Id, _dataContext, _mapper);

            serviceResponse.Data = quizBankResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> updateMultipeQuestionBank(CreateQuestionBankMultipeChoiceDTO updateQbMultiChoiceDTO, int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMultipeChoiceResponseDTO>();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == id);
            _mapper.Map(updateQbMultiChoiceDTO, quesToUpdate);

            await deleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();
            foreach (var item in updateQbMultiChoiceDTO.Answers)
            {
                createAnswer(item, id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public QuizbankAnswer createAnswer(QuestionBankAnswerDTO answer, int quizBankId)
        {
            answer.QuizBankId = quizBankId;

            QuizbankAnswer answerSave = _mapper.Map<QuizbankAnswer>(answer);
            _dataContext.QuizbankAnswers.Add(answerSave);

            return answerSave;
        }

        public async Task<bool> deleteTagAndAnswer(int quizBankId)
        {
            var dbAnswers = await _dataContext.QuizbankAnswers.Where(c => c.QuizBankId.Equals(quizBankId)).ToListAsync();
            foreach (var item in dbAnswers)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QuizbankAnswers.UpdateRange(dbAnswers);

            var dbQbTags = await _dataContext.QbTags.Where(c => c.QbId.Equals(quizBankId)).ToListAsync();
            foreach (var item in dbQbTags)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QbTags.UpdateRange(dbQbTags);

            return true;
        }

        public async Task<ServiceResponse<QuestionBankMultipeChoiceResponseDTO>> deleteMultipeQuestionBank(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMultipeChoiceResponseDTO>();

            QuizBank quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await deleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }
    }
}
