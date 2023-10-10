using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Services.ListQuestionServices;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class MatchingQuestionBankServicesImpl : IMatchingQuestionBankServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IQuestionBankList _qestionBanlListService;

        public MatchingQuestionBankServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IQuestionBankList questionBankList)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _qestionBanlListService = questionBankList;
        }

        public MatchingQuestionBankServicesImpl()
        {
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> CreateMatchingQuestionBank(CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO)
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
            questionMatchingResDto.MatchSubAnswers = SwapToMatchAnswerResponse(matchSubDtos);

            await _qestionBanlListService.CreateMultiQuestions(new List<int>{ quesSaved.Id });

            serviceResponse.Message = "Tạo câu hỏi thành công";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> GetMatchSubsQuestionBankById(int questionBankID)
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
            questionMatchingResDto.MatchSubAnswers = SwapToMatchAnswerResponse(matchSubDtos);
            questionMatchingResDto.addTags(questionBankID, _dataContext, _mapper);

            serviceResponse.Message = "OK";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> UpdateMatchSubsQuestionBank(CreateQuestionBankMatchingDTO updateQuestionBankMatchingDTO, int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var questionMatchingResDto = new QuestionBankMatchingResponseDTO();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == questionBankID);
            _mapper.Map(updateQuestionBankMatchingDTO, quesToUpdate);

            await DeleteRelationShip(questionBankID);
            await _dataContext.SaveChangesAsync();

            questionMatchingResDto = _mapper.Map<QuestionBankMatchingResponseDTO>(quesToUpdate);

            var matchSubs = createMatchSubQuestion(updateQuestionBankMatchingDTO.MatchSubs.ToList(), questionBankID);
            await _dataContext.SaveChangesAsync();

            var matchSubDtos = _mapper.Map<List<MatchSubQuestionBankDTO>>(matchSubs);

            questionMatchingResDto.MatchSubQuestions = _mapper.Map<List<MatchSubQuestionBankResponseDTO>>(matchSubs);
            questionMatchingResDto.MatchSubAnswers = SwapToMatchAnswerResponse(matchSubDtos);

            serviceResponse.Message = "Cập nhât câu hỏi thành công";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> DeleteMatchSubsQuestionBank(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();

            var quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(questionBankID));
            quesSaved.IsDeleted = 1;

            await DeleteRelationShip(questionBankID);
            await _dataContext.SaveChangesAsync();

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        private List<MatchSubQuestionBank> createMatchSubQuestion (List<CreateMatchSubQuestionBankDTO> createMatchSubQuestionBankDTOs, int questionBankID)
        {
            var matchSubQuestionBanks= _mapper.Map<List<MatchSubQuestionBank>>(createMatchSubQuestionBankDTOs);

            matchSubQuestionBanks.ForEach(e =>
            {
                e.QuestionBankId = questionBankID;
            });

            _dataContext.MatchSubQuestionBanks.AddRange(matchSubQuestionBanks);

            return matchSubQuestionBanks;
        }

        private List<String> SwapToMatchAnswerResponse (List<MatchSubQuestionBankDTO> matchSubQuestionBankDTOs)
        {
            var matchSubRes = new List<String>();

            matchSubQuestionBankDTOs.ForEach(e =>
            {
                matchSubRes.Add(e.AnswerText);
            });

            return matchSubRes;
        }
        public async Task<bool> DeleteRelationShip(int questionBankID)
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
