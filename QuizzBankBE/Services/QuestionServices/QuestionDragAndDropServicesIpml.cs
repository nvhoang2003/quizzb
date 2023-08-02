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
    public class QuestionDragAndDropServicesIpml : IDragAndDropQuestion
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuestionDragAndDropServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<DragAndDropQuestionDTO>> createDDQuestion(CreateDragAndDropDTO createQuestionBankMatchingDTO)
        {
            var serviceResponse = new ServiceResponse<DragAndDropQuestionDTO>();

            var sortedChoice = createQuestionBankMatchingDTO.Choice.OrderBy(c => c.Position).ToList();
            updateContent(createQuestionBankMatchingDTO, sortedChoice);

            Question quesSaved = _mapper.Map<Question>(createQuestionBankMatchingDTO);
            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            foreach (var item in sortedChoice)
            {
                createAnswer(item, quesSaved.Id);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo câu hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<DragAndDropQuestionDTO>> deleteDDQuestion(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<DragAndDropQuestionDTO>();

            Question quesSaved = await _dataContext.Questions.FirstOrDefaultAsync(c => c.Id.Equals(questionBankID));
            quesSaved.IsDeleted = 1;

            _dataContext.Questions.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await deleteTagAndAnswer(questionBankID);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<DragAndDropQuestionDTO>> getDDQuestionById(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<DragAndDropQuestionDTO>();
            var question = await _dataContext.Questions.FirstOrDefaultAsync(c => c.Id == questionBankID && c.QuestionsType == "DragAndDropIntoText");

            if (question == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            DragAndDropQuestionDTO questionResponse = _mapper.Map<DragAndDropQuestionDTO>(question);
            questionResponse.Answers = _dataContext.QuestionAnswers.Select(u => _mapper.Map<QuestionBankAnswerDTO>(u)).Where(c => c.QuestionId.Equals(questionBankID)).ToList();

            serviceResponse.Data = questionResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<DragAndDropQuestionDTO>> updateDDQuestion(CreateDragAndDropDTO updateQuestionBankMatchingDTO, int questionBankID)
        {
            var serviceResponse = new ServiceResponse<DragAndDropQuestionDTO>();
            var sortedChoice = updateQuestionBankMatchingDTO.Choice.OrderBy(c => c.Position).ToList();
            updateContent(updateQuestionBankMatchingDTO, sortedChoice);

            var quesToUpdate = _dataContext.Questions.FirstOrDefault(c => c.Id == questionBankID);
            _mapper.Map(updateQuestionBankMatchingDTO, quesToUpdate);

            await deleteTagAndAnswer(questionBankID);
            await _dataContext.SaveChangesAsync();
            foreach (var item in sortedChoice)
            {
                createAnswer(item, questionBankID);
            }

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public QuestionAnswer createAnswer(QuestionChoice choice, int questionId)
        {
            QuestionAnswerDTO answer = choice.Answer;
            answer.QuestionId = questionId;

            QuestionAnswer answerSave = _mapper.Map<QuestionAnswer>(answer);
            _dataContext.QuestionAnswers.Add(answerSave);

            return answerSave;
        }

        public async Task<bool> deleteTagAndAnswer(int questionId)
        {
            var dbAnswers = await _dataContext.QuestionAnswers.Where(c => c.QuestionId.Equals(questionId)).ToListAsync();
            foreach (var item in dbAnswers)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QuestionAnswers.UpdateRange(dbAnswers);

            var dbQbTags = await _dataContext.QbTags.Where(c => c.QbId.Equals(questionId)).ToListAsync();
            foreach (var item in dbQbTags)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QbTags.UpdateRange(dbQbTags);

            return true;
        }

        public void updateContent(CreateDragAndDropDTO createQuestionBankMatchingDTO, List<QuestionChoice> sortedChoice)
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
