using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class MatchingQuestionBankServicesImpl : IMatchingQuestionBankServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public MatchingQuestionBankServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public MatchingQuestionBankServicesImpl()
        {
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> createMatchingQuestionBank(CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var questionMatchingResDto = new QuestionBankMatchingResponseDTO();

            var quesSaved = _mapper.Map<QuizBank>(createQuestionBankMatchingDTO);

            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            questionMatchingResDto = _mapper.Map<QuestionBankMatchingResponseDTO>(quesSaved);

            var matchSubs = createMatchSubQuestion(createQuestionBankMatchingDTO.MatchSubs.ToList(), quesSaved.Id);
            await _dataContext.SaveChangesAsync();

            var matchSubDtos = _mapper.Map<List<MatchSubQuestionBankDTO>>(matchSubs);

            questionMatchingResDto.MatchSubQuestions = _mapper.Map<List<MatchSubQuestionBankResponseDTO>>(matchSubs);
            questionMatchingResDto.MatchSubAnswers = swapToMatchAnswerRes(matchSubDtos);

            serviceResponse.Message = "Tạo câu hỏi thành công";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> getMatchSubsQuestionBankById(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var questionBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == questionBankID && c.QuestionsType == "Match");

            if (questionBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            var questionMatchingResDto = _mapper.Map<QuestionBankMatchingResponseDTO>(questionBank);
            var dbMatchSubs = await _dataContext.MatchSubQuestionBanks.Where(c => c.QuestionBankId.Equals(questionBankID)).ToListAsync();
            var matchSubDtos = _mapper.Map<List<MatchSubQuestionBankDTO>>(dbMatchSubs);

            questionMatchingResDto.MatchSubQuestions = _mapper.Map<List<MatchSubQuestionBankResponseDTO>>(dbMatchSubs);
            questionMatchingResDto.MatchSubAnswers = swapToMatchAnswerRes(matchSubDtos);
            questionMatchingResDto.addTags(questionBankID, _dataContext, _mapper);

            serviceResponse.Message = "OK";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> updateMatchSubsQuestionBank(CreateQuestionBankMatchingDTO updateQuestionBankMatchingDTO, int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var questionMatchingResDto = new QuestionBankMatchingResponseDTO();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == questionBankID);
            var questSaved = _mapper.Map(updateQuestionBankMatchingDTO, quesToUpdate);

            _dataContext.QuizBanks.Update(questSaved);
            await _dataContext.SaveChangesAsync();

            await deleteRelationShip(questionBankID);
            await _dataContext.SaveChangesAsync();

            questionMatchingResDto = _mapper.Map<QuestionBankMatchingResponseDTO>(quesToUpdate);

            var matchSubs = createMatchSubQuestion(updateQuestionBankMatchingDTO.MatchSubs.ToList(), questionBankID);
            await _dataContext.SaveChangesAsync();

            var matchSubDtos = _mapper.Map<List<MatchSubQuestionBankDTO>>(matchSubs);

            questionMatchingResDto.MatchSubQuestions = _mapper.Map<List<MatchSubQuestionBankResponseDTO>>(matchSubs);
            questionMatchingResDto.MatchSubAnswers = swapToMatchAnswerRes(matchSubDtos);

            serviceResponse.Message = "Cập nhât câu hỏi thành công";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> deleteMatchSubsQuestionBank(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();

            var quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(questionBankID));
            quesSaved.IsDeleted = 1;

            await deleteRelationShip(questionBankID);
            await _dataContext.SaveChangesAsync();

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        private List<MatchSubQuestionBank> createMatchSubQuestion (List<MatchSubQuestionBankDTO> matchSubQuestionBankDTOs, int questionBankID)
        {
            var matchSubQuestionBanks= _mapper.Map<List<MatchSubQuestionBank>>(matchSubQuestionBankDTOs);

            matchSubQuestionBanks.ForEach(e =>
            {
                e.QuestionBankId = questionBankID;
            });

            _dataContext.MatchSubQuestionBanks.AddRange(matchSubQuestionBanks);

            return matchSubQuestionBanks;
        }

        private List<String> swapToMatchAnswerRes (List<MatchSubQuestionBankDTO> matchSubQuestionBankDTOs)
        {
            var matchSubRes = new List<String>();

            matchSubQuestionBankDTOs.ForEach(e =>
            {
                matchSubRes.Add(e.AnswerText);
            });

            return matchSubRes;
        }
        public async Task<bool> deleteRelationShip(int questionBankID)
        {
            var dbMatchSubs = await _dataContext.MatchSubQuestionBanks.Where(c => c.QuestionBankId.Equals(questionBankID)).ToListAsync();
            dbMatchSubs.ForEach(e =>
            {
                e.IsDeleted = 1;
            });

            _dataContext.MatchSubQuestionBanks.UpdateRange(dbMatchSubs);

            var dbQbTags = await _dataContext.QbTags.Where(c => c.QbId.Equals(questionBankID)).ToListAsync();

            dbQbTags.ForEach(e =>
            {
                e.IsDeleted = 1;
            });

            _dataContext.QbTags.UpdateRange(dbQbTags);

            return true;
        }
    }
}
