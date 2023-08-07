using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionServices
{
    public class TrueFalseQuestionService : ITrueFalseQuestionService
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public TrueFalseQuestionService(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public TrueFalseQuestionService()
        {
        }

        public async Task<ServiceResponse<TrueFalseQuestionDTO>> createNewTrueFalseQuestion(CreateQuestionTrueFalseDTO createQuestionTFDTO)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionDTO>();

            Question quesSaved = _mapper.Map<Question>(createQuestionTFDTO);
            _dataContext.Questions.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            createAnswer(createQuestionTFDTO, quesSaved.Id);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Tạo Câu Hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<TrueFalseQuestionDTO>> getTrueFalseQuestionById(int Id)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionDTO>();
            var question = await _dataContext.Questions.FirstOrDefaultAsync(c => c.Id == Id && c.QuestionsType == "TrueFalse");

            if (question == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            TrueFalseQuestionDTO questionResponse = _mapper.Map<TrueFalseQuestionDTO>(question);
            var dbAnswers = await _dataContext.QuestionAnswers.ToListAsync();

            questionResponse.Answers = dbAnswers.Select(u => _mapper.Map<QuestionAnswerDTO>(u)).Where(c => c.QuestionId.Equals(Id)).ToList();

            serviceResponse.Data = questionResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<TrueFalseQuestionDTO>> updateTrueFalseQuestion(CreateQuestionTrueFalseDTO updateQbTrueFalseDTO, int id)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionDTO>();

            var quesToUpdate = _dataContext.Questions.FirstOrDefault(c => c.Id == id);
            _mapper.Map(updateQbTrueFalseDTO, quesToUpdate);

            await deleteAnswer(id);
            await _dataContext.SaveChangesAsync();
            createAnswer(updateQbTrueFalseDTO, id);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<TrueFalseQuestionDTO>> deleteTrueFalseQuestion(int id)
        {
            var serviceResponse = new ServiceResponse<TrueFalseQuestionDTO>();

            Question quesSaved = _dataContext.Questions.FirstOrDefault(c => c.Id.Equals(id));
            quesSaved.IsDeleted = 1;

            _dataContext.Questions.Update(quesSaved);
            await _dataContext.SaveChangesAsync();

            await deleteAnswer(id);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xóa câu hỏi thành công");
            return serviceResponse;
        }

        public async Task<bool> deleteAnswer(int quizBankId)
        {
            var dbAnswers = await _dataContext.QuestionAnswers.Where(c => c.QuestionId.Equals(quizBankId)).ToListAsync();
            foreach (var item in dbAnswers)
            {
                item.IsDeleted = 1;
            }
            _dataContext.QuestionAnswers.UpdateRange(dbAnswers);

            return true;
        }

        public  QuestionAnswer createAnswer(CreateQuestionTrueFalseDTO answer, int questionId)
        {
            List<QuestionAnswer> qa = new List<QuestionAnswer>();
            QuestionAnswer rightAnswer =  new QuestionAnswer();
            defineAnswer(rightAnswer, 1, answer.RightAnswer.ToString(), questionId);
            qa.Add(rightAnswer);

            string wrongAnswerContent = answer.RightAnswer == true ? "False" : "True";
            QuestionAnswer wrongAnswer = new QuestionAnswer();
            defineAnswer(wrongAnswer, 0, wrongAnswerContent, questionId);
            qa.Add(wrongAnswer);

            _dataContext.QuestionAnswers.AddRange(qa);

            return rightAnswer;
        }

        public void defineAnswer(QuestionAnswer answer, int fraction, string content, int questionId)
        {
            answer.Fraction = fraction;
            answer.Content = content;
            answer.QuestionId = questionId;
        }
    }
}
