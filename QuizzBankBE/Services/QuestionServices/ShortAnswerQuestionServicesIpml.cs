using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
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

        public async Task<ServiceResponse<QuestionBankShortAnswerDTO>> createSAQuestionBank(CreateQuestionBankShortAnswerDTO createQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<QuestionBankShortAnswerDTO>();

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in createQuestionBankDTO.Answers)
            {
                createAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankShortAnswerDTO>> deleteSAQuestionBank(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankShortAnswerDTO>();

            QuizBank quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await deleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankShortAnswerDTO>> getSAQuestionBankById(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankShortAnswerDTO>();
            var quizBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == id && c.QuestionsType == "ShortAnswer");

            if (quizBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            QuestionBankShortAnswerDTO quizBankResponse = _mapper.Map<QuestionBankShortAnswerDTO>(quizBank);
            var dbAnswers = await _dataContext.QuizbankAnswers.ToListAsync();

            quizBankResponse.Answers = dbAnswers.Select(u => _mapper.Map<QuestionBankAnswerDTO>(u)).Where(c => c.QuizBankId.Equals(id)).ToList();
            quizBankResponse.Tags = (from q in _dataContext.QuizBanks
                                     join qt in _dataContext.QbTags on q.Id equals qt.QbId
                                     join t in _dataContext.Tags on qt.TagId equals t.Id
                                     where q.Id == id
                                     select t).Distinct().ToList();

            serviceResponse.Data = quizBankResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankShortAnswerDTO>> updateSAQuestionBank(CreateQuestionBankShortAnswerDTO updateQuestionDTO, int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankShortAnswerDTO>();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == id);
            _mapper.Map(updateQuestionDTO, quesToUpdate);

            await deleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();
            foreach (var item in updateQuestionDTO.Answers)
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
    }
}
