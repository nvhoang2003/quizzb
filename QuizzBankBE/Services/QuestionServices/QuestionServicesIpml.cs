using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using System.Linq;

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

        public async Task<ServiceResponse<PageList<QuestionBankEntryResponseDTO>>> getListQuestion(OwnerParameter ownerParameters, int questionCategoryId)
        {
            var serviceResponse = new ServiceResponse<PageList<QuestionBankEntryResponseDTO>>();

            var dbQuestion = await _dataContext.QuestionBankEntries.ToListAsync();
            var questionBankEntriesDTO = dbQuestion.Select(u => _mapper.Map<QuestionBankEntryResponseDTO>(u)).
                Where(q => q.QuestionCategoryId == questionCategoryId).ToList();

            foreach (var item in questionBankEntriesDTO)
            {
                await getQuestionAndAnswerMaxVersion(item);
            }

            serviceResponse.Data = PageList<QuestionBankEntryResponseDTO>.ToPageList(
            questionBankEntriesDTO.AsEnumerable<QuestionBankEntryResponseDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankEntryResponseDTO>> getQuestionById(int questionbankEntryId)
        {
            var serviceResponse = new ServiceResponse<QuestionBankEntryResponseDTO>();

            var questionBankEntryDataContext = await _dataContext.QuestionBankEntries.ToListAsync();
            var questionBankEntryDTO = questionBankEntryDataContext.
                Where(q => q.IdquestionBankEntry == questionbankEntryId).
                Select(u => _mapper.Map<QuestionBankEntryResponseDTO>(u)).
                FirstOrDefault();

            if (questionBankEntryDTO == null)
            {
                serviceResponse.Status = false;
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Câu hỏi không tồn tại !";
                return serviceResponse;
            }

            await getQuestionAndAnswerMaxVersion(questionBankEntryDTO);

            serviceResponse.Data = questionBankEntryDTO;
            serviceResponse.Status = true;
            serviceResponse.StatusCode = 200;
            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionResponseDTO>> updateQuestion(CreateQuestionDTO updateQuestionDTO,int id)
        {
            // Do làm tính năng backup nên khi sửa sẽ Tạo nội dung câu hỏi mới(bao gồm đáp án) và question version mới)
            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

            Question quesSaved = _mapper.Map<Question>(updateQuestionDTO);
            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            await createNewQuestionVersion(quesSaved.Idquestions, id);

            serviceResponse.Status = true;
            serviceResponse.StatusCode = 200;
            serviceResponse.Message = "Sửa thành công !";
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

        public async Task<bool> getQuestionAndAnswerMaxVersion(QuestionBankEntryResponseDTO questionBankEntry)
        {
            int questionId = (from qbe in _dataContext.QuestionBankEntries
                              join qv in _dataContext.QuestionVersions on qbe.IdquestionBankEntry equals qv.QuestionBankEntryId
                              join q in _dataContext.Questions on qv.QuestionId equals q.Idquestions
                              where qbe.IdquestionBankEntry == questionBankEntry.IdquestionBankEntry
                              orderby qv.Version descending
                              select q.Idquestions).FirstOrDefault();

            var dbQuestionItem = await _dataContext.Questions.ToListAsync();
            questionBankEntry.Question = dbQuestionItem.Select(u => _mapper.Map<QuestionDTO>(u)).
                Where(q => q.Idquestions == questionId).
                FirstOrDefault();

            var dbAnswer = await _dataContext.Answers.ToListAsync();
            questionBankEntry.Answers = dbAnswer.Select(u => _mapper.Map<QuestionAnswerDTO>(u)).
                Where(q => q.Questionid == questionId).
                ToList();

            return true;
        }
    }
}
