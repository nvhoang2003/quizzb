using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public class QuestionServicesIpml : IQuestionServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuestionServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public QuestionServicesIpml()
        {
        }

        public async Task<ServiceResponse<QuestionResponseDTO>> createNewQuestion(CreateQuestionDTO createQuestionDTO)
        {
            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

            int questionCategoryId = createQuestionDTO.QuestionBankEntry.QuestionCategoryId;
            var quesBankEntry = await createNewQuestionBankEntry(questionCategoryId);

            Question quesSaved = _mapper.Map<Question>(createQuestionDTO);
            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            await createNewQuestionVersion(quesSaved.Idquestions, quesBankEntry.IdquestionBankEntry);

            serviceResponse.Status = true;
            serviceResponse.StatusCode = 200;
            serviceResponse.Message = "Tạo thành công !";
            return serviceResponse;
        }

        public async Task<QuestionBankEntry> createNewQuestionBankEntry(int id)
        {
            QuestionBankEntryDTO quesBankEntry = new QuestionBankEntryDTO(id);

            QuestionBankEntry quesBankEntrySaved = _mapper.Map<QuestionBankEntry>(quesBankEntry);
            _dataContext.QuestionBankEntries.Add(quesBankEntrySaved);

            await _dataContext.SaveChangesAsync();
            return quesBankEntrySaved;
        }

        public async Task<QuestionVersionDTO> createNewQuestionVersion(int quesId, int quesEntryId)
        {
            var db = await _dataContext.QuestionVersions.ToListAsync();
            int version = db.Where(u => u.QuestionBankEntryId == quesEntryId).ToList().Count + 1;

            QuestionVersionDTO questionVersion = new QuestionVersionDTO(quesId, quesEntryId, "Ready", version);

            QuestionVersion quesVersionSaved = _mapper.Map<QuestionVersion>(questionVersion);
            _dataContext.QuestionVersions.Add(quesVersionSaved);

            await _dataContext.SaveChangesAsync();
            return questionVersion;
        }
    }
}
