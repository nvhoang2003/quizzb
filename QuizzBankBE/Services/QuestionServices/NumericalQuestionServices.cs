using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public class NumericalQuestionServices : INumericalQuestionService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public NumericalQuestionServices(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public NumericalQuestionServices()
        {
        }

        public async Task<ServiceResponse<NumericalQuestionDTO>> CreateNumericalQuestionBank(CreateNumericalQuestionDTO createQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<NumericalQuestionDTO>();

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            CreateAnswer(createQuestionBankDTO, quesSaved.Id);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<NumericalQuestionDTO>> GetNumericalQuestionBankById(int id)
        {
            var serviceResponse = new ServiceResponse<NumericalQuestionDTO>();
            var quizBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == id && c.QuestionsType == "Numerical");

            if (quizBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            NumericalQuestionDTO quizBankResponse = _mapper.Map<NumericalQuestionDTO>(quizBank);
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

        public async Task<ServiceResponse<NumericalQuestionDTO>> UpdateNumericalQuestionBank(CreateNumericalQuestionDTO updateQbNumericalDTO, int id)
        {
            var serviceResponse = new ServiceResponse<NumericalQuestionDTO>();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == id);
            _mapper.Map(updateQbNumericalDTO, quesToUpdate);

            await DeleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();
            CreateAnswer(updateQbNumericalDTO, id);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }


        public async Task<ServiceResponse<NumericalQuestionDTO>> DeleteNumericalQuestionBank(int id)
        {
            var serviceResponse = new ServiceResponse<NumericalQuestionDTO>();

            QuizBank quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await DeleteTagAndAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        public async Task<bool> DeleteTagAndAnswer(int quizBankId)
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


        public QuizbankAnswer CreateAnswer(CreateNumericalQuestionDTO answer, int quizBankId)
        {
            QuestionBankAnswerDTO rightAnswer = new QuestionBankAnswerDTO(1, answer.RightAnswers.ToString(), quizBankId);
            QuizbankAnswer answerSave = _mapper.Map<QuizbankAnswer>(rightAnswer);

            _dataContext.QuizbankAnswers.Add(answerSave);

            return answerSave;
        }

    }
}
