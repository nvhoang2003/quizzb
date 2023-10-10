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
    public class QuestionBankDragAndDropServicesIpml : IDragAndDropQuestionBank
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IQuestionBankList _qestionBanlListService;

        public QuestionBankDragAndDropServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IQuestionBankList questionBankList)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _qestionBanlListService = questionBankList;
        }
        public async Task<ServiceResponse<QBankDragAndDropDTO>> CreateDragDropQuestionBank(CreateQBankDragAndDropDTO createQuestionBankMatchingDTO)
        {
            var serviceResponse = new ServiceResponse<QBankDragAndDropDTO>();

            var sortedChoice = createQuestionBankMatchingDTO.Choice.OrderBy(c => c.Position).ToList();
            UpdateContent(createQuestionBankMatchingDTO, sortedChoice);

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankMatchingDTO);
            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in sortedChoice)
            {
                createAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();

            await _qestionBanlListService.CreateMultiQuestions(new List<int> { quesSaved.Id });

            serviceResponse.updateResponse(200, "Tạo câu hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<QBankDragAndDropDTO>> DeleteDragDropQuestionBank(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QBankDragAndDropDTO>();

            QuizBank quesSaved = _dataContext.QuizBanks.FirstOrDefault(c => c.Id.Equals(questionBankID));
            quesSaved.IsDeleted = 1;

            _dataContext.QuizBanks.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await DeleteTagAndAnswer(questionBankID);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<QBankDragAndDropDTO>> GetDragDropQuestionBankById(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QBankDragAndDropDTO>();
            var quizBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == questionBankID && c.QuestionsType == "DragAndDropIntoText");

            if (quizBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            QBankDragAndDropDTO quizBankResponse = _mapper.Map<QBankDragAndDropDTO>(quizBank);
            var dbAnswers = await _dataContext.QuizbankAnswers.ToListAsync();

            quizBankResponse.Answers = dbAnswers.Select(u => _mapper.Map<QuestionBankAnswerDTO>(u)).Where(c => c.QuizBankId.Equals(questionBankID)).ToList();
            quizBankResponse.addTags(questionBankID, _dataContext, _mapper);

            serviceResponse.Data = quizBankResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<QBankDragAndDropDTO>> UpdateDragDropQuestionBank(CreateQBankDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QBankDragAndDropDTO>();
            var sortedChoice = updateQuestionBankMatchingDTO.Choice.OrderBy(c => c.Position).ToList();
            UpdateContent(updateQuestionBankMatchingDTO, sortedChoice);

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == questionBankID);
            _mapper.Map(updateQuestionBankMatchingDTO, quesToUpdate);

            await DeleteTagAndAnswer(questionBankID);
            await _dataContext.SaveChangesAsync();
            foreach (var item in sortedChoice)
            {
                createAnswer(item, questionBankID);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public QuizbankAnswer createAnswer(Choice choice, int quizBankId)
        {
            QuestionBankAnswerDTO answer = choice.Answer;
            answer.QuizBankId = quizBankId;

            QuizbankAnswer answerSave = _mapper.Map<QuizbankAnswer>(answer);
            _dataContext.QuizbankAnswers.Add(answerSave);

            return answerSave;
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

        public void UpdateContent(CreateQBankDragAndDropDTO createQuestionBankMatchingDTO, List<Choice> sortedChoice)
        {
            for (int i = 0; i < sortedChoice.Count; i++)
            {
                int position = i + 1;
                if (sortedChoice[i].Position != position)
                {
                    createQuestionBankMatchingDTO.Content = createQuestionBankMatchingDTO.Content.Replace("[[" + sortedChoice[i].Position + "]]", "[[" + position + "]]");
                }
            }
        }
    }
}
