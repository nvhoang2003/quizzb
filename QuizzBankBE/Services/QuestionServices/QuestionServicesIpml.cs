//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using QuizzBankBE.DataAccessLayer.Data;
//using QuizzBankBE.DataAccessLayer.DataObject;
//using QuizzBankBE.DTOs;
//using QuizzBankBE.JWT;
//using QuizzBankBE.Model;
//using QuizzBankBE.Model.Pagination;
//using System.Linq;

//namespace QuizzBankBE.Services.QuestionServices
//{
//    public class QuestionServicesIpml : IQuestionServices
//    {
//        public DataContext _dataContext;
//        public IMapper _mapper;
//        public IConfiguration _configuration;
//        public readonly IjwtProvider _jwtProvider;

//        public QuestionServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
//        {
//            _dataContext = dataContext;
//            _mapper = mapper;
//            _jwtProvider = jwtProvider;
//            _configuration = configuration;
//        }

//        public QuestionServicesIpml()
//        {
//        }

//        public async Task<ServiceResponse<QuestionResponseDTO>> createNewQuestion(CreateQuestionDTO createQuestionDTO)
//        {
//            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

//            int questionCategoryId = createQuestionDTO.QuestionBankEntry.QuestionCategoryId;
//            var quesBankEntry = await createNewQuestionBankEntry(questionCategoryId);

//            Question quesSaved = _mapper.Map<Question>(createQuestionDTO);
//            _dataContext.Questions.Add(quesSaved);
//            await _dataContext.SaveChangesAsync();

//            await createNewQuestionVersion(quesSaved.Idquestions, quesBankEntry.IdquestionBankEntry);

//            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");
//            return serviceResponse;
//        }

//        public async Task<ServiceResponse<QuestionCategoryDTO>> getListQuestion(OwnerParameter ownerParameters, int questionCategoryId)
//        {
//            var serviceResponse = new ServiceResponse<QuestionCategoryDTO>();

//            var categoryDb = await _dataContext.QuestionCategories.FirstOrDefaultAsync(q => q.IdquestionCategories == questionCategoryId);

//            if (categoryDb == null)
//            {
//                serviceResponse.updateResponse(404, "Không tồn tại");
//            }
//            else
//            {
//                var catDTO = _mapper.Map<QuestionCategoryDTO>(categoryDb);

//                var dbQuestion = await _dataContext.QuestionBankEntries.ToListAsync();
//                var questionBankEntriesDTO = dbQuestion.Select(u => _mapper.Map<QuestionBankEntryResponseDTO>(u)).
//                    Where(q => q.QuestionCategoryId == questionCategoryId).ToList();

//                foreach (var item in questionBankEntriesDTO)
//                {
//                    await getQuestionAndAnswerMaxVersion(item);
//                    catDTO.QuestionBankEntries.Add(item);
//                }

//                serviceResponse.updateResponse(200, "Ok");
//                serviceResponse.Data = catDTO;
//            }

//            return serviceResponse;
//        }

//        public async Task<ServiceResponse<QuestionBankEntryResponseDTO>> getQuestionById(int questionbankEntryId)
//        {
//            var serviceResponse = new ServiceResponse<QuestionBankEntryResponseDTO>();

//            var questionBankEntryDataContext = await _dataContext.QuestionBankEntries.ToListAsync();
//            var questionBankEntryDTO = questionBankEntryDataContext.
//                Where(q => q.IdquestionBankEntry == questionbankEntryId).
//                Select(u => _mapper.Map<QuestionBankEntryResponseDTO>(u)).
//                FirstOrDefault();

//            if (questionBankEntryDTO == null)
//            {
//                serviceResponse.Status = false;
//                serviceResponse.StatusCode = 400;
//                serviceResponse.Message = "Câu hỏi không tồn tại !";
//                serviceResponse.updateResponse(400, "Câu hỏi không tồn tại");
//                return serviceResponse;
//            }

//            await getQuestionAndAnswerMaxVersion(questionBankEntryDTO);

//            serviceResponse.Data = questionBankEntryDTO;
//            serviceResponse.updateResponse(200, "");
//            return serviceResponse;
//        }

//        public async Task<ServiceResponse<QuestionResponseDTO>> updateQuestion(CreateQuestionDTO updateQuestionDTO,int id)
//        {
//            // Do làm tính năng backup nên khi sửa sẽ Tạo nội dung câu hỏi mới(bao gồm đáp án) và question version mới)
//            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

//            Question quesSaved = _mapper.Map<Question>(updateQuestionDTO);
//            _dataContext.Questions.Add(quesSaved);
//            await _dataContext.SaveChangesAsync();

//            await createNewQuestionVersion(quesSaved.Idquestions, id);

//            serviceResponse.updateResponse(200, "Sửa thành công");
//            return serviceResponse;
//        }

//        public async Task<ServiceResponse<QuestionResponseDTO>> deleteQuestion(int id)
//        {
//            var serviceResponse = new ServiceResponse<QuestionResponseDTO>();

//            var dbQuestionBankEntry = await _dataContext.QuestionBankEntries.FirstOrDefaultAsync(q => q.IdquestionBankEntry == id);

//            var dbQuestionVersion = await getLatesVersion(id);
//            int? questionId = dbQuestionVersion?.QuestionId;
//            var dbQuestion = await _dataContext.Questions.FirstOrDefaultAsync(q => q.Idquestions == questionId);

//            if (dbQuestion == null || dbQuestionVersion == null)
//            {
//                serviceResponse.updateResponse(400, "Câu hỏi không tồn tại");
//                return serviceResponse;
//            }

//            dbQuestionVersion.IsDeleted = 1;
//            _dataContext.QuestionVersions.Update(dbQuestionVersion);

//            dbQuestion.IsDeleted = 1;
//            _dataContext.Questions.Update(dbQuestion);

//            await _dataContext.SaveChangesAsync();

//            var checkVersion = await getLatesVersion(id);
//            if (checkVersion == null)
//            {
//                dbQuestionBankEntry.IsDeleted = 1;
//                _dataContext.QuestionBankEntries.Update(dbQuestionBankEntry);
//                await _dataContext.SaveChangesAsync();
//            }

//            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");

//            return serviceResponse;
//        }

//     /*   public async Task<QuestionBankEntry> createNewQuestionBankEntry(int id)
//        {
//            QuestionBankEntryDTO quesBankEntry = new QuestionBankEntryDTO(id);

//            QuestionBankEntry quesBankEntrySaved = _mapper.Map<QuestionBankEntry>(quesBankEntry);
//            _dataContext.QuestionBankEntries.Add(quesBankEntrySaved);

//            await _dataContext.SaveChangesAsync();
//            return quesBankEntrySaved;
//        }*/

//        public async Task<QuestionVersionDTO> createNewQuestionVersion(int quesId, int quesEntryId)
//        {
//            var db = await _dataContext.QuestionVersions.ToListAsync();
//            int version = db.Where(u => u.QuestionBankEntryId == quesEntryId).ToList().Count + 1;

//            QuestionVersionDTO questionVersion = new QuestionVersionDTO(quesId, quesEntryId, "Ready", version);

//            QuestionVersion quesVersionSaved = _mapper.Map<QuestionVersion>(questionVersion);
//            _dataContext.QuestionVersions.Add(quesVersionSaved);

//            await _dataContext.SaveChangesAsync();
//            return questionVersion;
//        }

//        public async Task<QuestionVersion> getLatesVersion(int id)
//        {
//           var questionVersion = await (from qbe in _dataContext.QuestionBankEntries
//                              join qv in _dataContext.QuestionVersions on qbe.IdquestionBankEntry equals qv.QuestionBankEntryId
//                              join q in _dataContext.Questions on qv.QuestionId equals q.Idquestions
//                              where qbe.IdquestionBankEntry == id && qv.IsDeleted == 0
//                              orderby qv.Version descending
//                              select qv).FirstOrDefaultAsync();
//            return questionVersion;
//        }

//        public async Task<bool> getQuestionAndAnswerMaxVersion(QuestionBankEntryResponseDTO questionBankEntry)
//        {
//            var questionVersion = await getLatesVersion(questionBankEntry.IdquestionBankEntry);
//            int questionId = questionVersion.QuestionId;

//            var dbQuestionItem = await _dataContext.Questions.ToListAsync();
//            questionBankEntry.Question = dbQuestionItem.Select(u => _mapper.Map<QuestionDTO>(u)).
//                Where(q => q.Idquestions == questionId).
//                FirstOrDefault();

//            var dbAnswer = await _dataContext.Answers.ToListAsync();
//            questionBankEntry.Answers = dbAnswer.Select(u => _mapper.Map<QuestionAnswerDTO>(u)).
//                Where(q => q.Questionid == questionId).
//                ToList();

//            return true;
//        }
//    }
//}
