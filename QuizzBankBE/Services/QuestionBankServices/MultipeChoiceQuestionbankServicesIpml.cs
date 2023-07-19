using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

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

            //int questionCategoryId = createQuestionDTO.QuestionBankEntry.QuestionCategoryId;
            //var quesBankEntry = await createNewQuestionBankEntry(questionCategoryId);
            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            //await createNewQuestionVersion(quesSaved.Idquestions, quesBankEntry.IdquestionBankEntry);

            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");
            return serviceResponse;
        }
    }
}
