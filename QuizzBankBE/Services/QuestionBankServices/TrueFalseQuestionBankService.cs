using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using static QuizzBankBE.DTOs.QuestionDTO;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class TrueFalseQuestionBankService : ITrueFalseQuestionBankService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public TrueFalseQuestionBankService(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public TrueFalseQuestionBankService()
        {
        }

        public async Task<ServiceResponse<TrueFalseQuestionBankDTO>> createNewTrueFalseQuestionBank(CreateTrueFalseQuestionDTO createQuestionTFDTO)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionBankDTO>();

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionTFDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in createQuestionTFDTO.Answers)
            {
                createAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<TrueFalseQuestionBankDTO>> getTrueFalseQuestionBankById(int Id)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionBankDTO>();
            var quizBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == Id);

            if (quizBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            TrueFalseQuestionBankDTO quizBankResponse = _mapper.Map<TrueFalseQuestionBankDTO>(quizBank);
            var dbAnswers = await _dataContext.QuizbankAnswers.ToListAsync();

            quizBankResponse.Answers = dbAnswers.Select(u => _mapper.Map<QuestionBankAnswerDTO>(u)).Where(c => c.QuizBankId.Equals(Id)).ToList();
            quizBankResponse.Tags = (from q in _dataContext.QuizBanks
                                     join qt in _dataContext.QbTags on q.Id equals qt.QbId
                                     join t in _dataContext.Tags on qt.TagId equals t.Id
                                     where q.Id == Id
                                     select t).Distinct().ToList();

            serviceResponse.Data = quizBankResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<TrueFalseQuestionBankDTO>> updateTrueFalseQuestionBank(CreateTrueFalseQuestionDTO updateQbTrueFalseDTO, int id)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionBankDTO>();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == id);
            _mapper.Map(updateQbTrueFalseDTO, quesToUpdate);

            await deleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();
            foreach (var item in updateQbTrueFalseDTO.Answers)
            {
                createAnswer(item, id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<TrueFalseQuestionBankDTO>> deleteTrueFalseQuestionBank(int id)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionBankDTO>();

            QuizBank quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await deleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
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

        public QuizbankAnswer createAnswer(CreateAnswerTrueFalseDTO answer, int quizBankId)
        {
            answer.QuizBankId = quizBankId;
            
            QuizbankAnswer answerSave = _mapper.Map<QuizbankAnswer>(answer);
            answerSave.Content = answer.Content.ToString();
            _dataContext.QuizbankAnswers.Add(answerSave);

            return answerSave;
        }
    }
}
