using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class MatchingQuestionBankServicesImpl
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

            var quesSaved = _mapper.Map<QuizBank>(createQuestionBankMatchingDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            _dataContext.MatchSubQuestionBanks.AddRange(_mapper.Map<List<MatchSubQuestionBank>>(createQuestionBankMatchingDTO.MatchSubs));
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Tạo Câu Hỏi thành công";
            serviceResponse.Data = _mapper.Map<QuestionBankMatchingResponseDTO>(createQuestionBankMatchingDTO);

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> getMatchSubsQuestionBankById(int Id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var quizBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == Id && c.QuestionsType == "MultiChoice");

            if (quizBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            var quizBankResponse = _mapper.Map<QuestionBankMatchingResponseDTO>(quizBank);
            var dbMatchSubs = await _dataContext.MatchSubQuestionBanks.Where(c => c.QuestionBankId.Equals(Id)).ToListAsync();

            quizBankResponse.MatchSubs = _mapper.Map<List<MatchSubQuestionBankDTO>>(dbMatchSubs);
            quizBankResponse.addTags(Id, _dataContext, _mapper);

            serviceResponse.Data = quizBankResponse;
            return serviceResponse;
        }
    }
}
