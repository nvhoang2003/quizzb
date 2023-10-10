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
    public class MatchingQuestionServicesImpl : IMatchingQuestionServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public MatchingQuestionServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public MatchingQuestionServicesImpl()
        {
        }

        public async Task<ServiceResponse<MatchQuestionDTO>> CreateMatchingQuestion(CreateMatchQuestionDTO createQuestionMatchingDTO)
        {
            var serviceResponse = new ServiceResponse<MatchQuestionDTO>();

            var quesSaved = _mapper.Map<Question>(createQuestionMatchingDTO);

            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            CreateMatchSubQuestion(createQuestionMatchingDTO.MatchSubQuestion.ToList(), quesSaved.Id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Tạo câu hỏi thành công";
            return serviceResponse;
        }

        public async Task<ServiceResponse<MatchQuestionDTO>> GetMatchSubsQuestionById(int questionID)
        {
            var serviceResponse = new ServiceResponse<MatchQuestionDTO>();
            var questionBank = await _dataContext.Questions.FirstOrDefaultAsync(c => c.Id == questionID && c.QuestionsType == "Match");

            if (questionBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            var questionMatchingResDto = _mapper.Map<MatchQuestionDTO>(questionBank);
            var dbMatchSubs = await _dataContext.MatchSubQuestions.Where(c => c.QuestionId.Equals(questionID)).ToListAsync();
            questionMatchingResDto.MatchSubQuestion = _mapper.Map<List<MatchSubQuestionResponseDTO>>(dbMatchSubs);

            serviceResponse.Message = "OK";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<MatchQuestionDTO>> DeleteMatchSubsQuestion(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<MatchQuestionDTO>();

            var quesSaved = _dataContext.Questions.FirstOrDefault(c => c.Id.Equals(questionBankID));
            quesSaved.IsDeleted = 1;

            await DeleteRelationShip(questionBankID);
            await _dataContext.SaveChangesAsync();

            _dataContext.Questions.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        private List<MatchSubQuestion> CreateMatchSubQuestion (List<CreateMatchSubQuestionDTO> matchSubQuestionBankDTOs, int questionBankID)
        {
            var matchSubQuestionBanks= _mapper.Map<List<MatchSubQuestion>>(matchSubQuestionBankDTOs);

            matchSubQuestionBanks.ForEach(e =>
            {
                e.QuestionId = questionBankID;
            });

            _dataContext.MatchSubQuestions.AddRange(matchSubQuestionBanks);

            return matchSubQuestionBanks;
        }

        public async Task<bool> DeleteRelationShip(int questionBankID)
        {
            var dbMatchSubs = await _dataContext.MatchSubQuestions.Where(c => c.QuestionId.Equals(questionBankID)).ToListAsync();
            dbMatchSubs.ForEach(e =>
            {
                e.IsDeleted = 1;
            });

            _dataContext.MatchSubQuestions.UpdateRange(dbMatchSubs);

            return true;
        }
    }
}
