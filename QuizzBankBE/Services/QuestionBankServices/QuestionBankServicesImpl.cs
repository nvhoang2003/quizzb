using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Services.ListQuestionServices;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class QuestionBankServicesImpl : IQuestionBankServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IQuestionBankList _qestionBanlListService;

        public QuestionBankServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IQuestionBankList questionBankList)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _qestionBanlListService = questionBankList;
        }
        public async Task<ServiceResponse<CreateQuestionBankDTO>> CreateQuestionBank(CreateQuestionBankDTO createQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            if(createQuestionBankDTO.Questionstype == "TrueFalse")
            {
                GenerateAnswerTrueFalseQuestion(createQuestionBankDTO);
            }
            if(createQuestionBankDTO.Questionstype == "DragAndDropIntoText")
            {
                UpdateContentDragAndDropQuestion(createQuestionBankDTO);
            }

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankDTO);
            _dataContext.QuizBanks.AddRange(quesSaved);
            await _dataContext.SaveChangesAsync();

            await _qestionBanlListService.CreateMultiQuestions(new List<int> { quesSaved.Id });

            serviceResponse.updateResponse(200, "Tạo Thành Công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankResponseDTO>> GetQuestionBankById(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankResponseDTO>();

            var quesResponse = new QuestionBankResponseDTO();

            var ques = await _dataContext.QuizBanks
               .Include(i => i.QuizbankAnswers)
               .Include(i => i.MatchSubQuestionBanks)
               .Include(i => i.QbTags)
               .ThenInclude(i => i.Tag)
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync();

            if (ques == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            quesResponse = _mapper.Map<QuestionBankResponseDTO>(ques);
            quesResponse.Tags = _mapper.Map<List<TagDTO>>(ques.QbTags.Select(q => q.Tag).ToList().Distinct());

            serviceResponse.Data = quesResponse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<CreateQuestionBankDTO>> UpdateQuestionBank(int questionBankID, CreateQuestionBankDTO updateQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            if (updateQuestionBankDTO.Questionstype == "TrueFalse")
            {
                GenerateAnswerTrueFalseQuestion(updateQuestionBankDTO);
            }
            if (updateQuestionBankDTO.Questionstype == "DragAndDropIntoText")
            {
                UpdateContentDragAndDropQuestion(updateQuestionBankDTO);
            }

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == questionBankID);
            _mapper.Map(updateQuestionBankDTO, quesToUpdate);

            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public void UpdateContentDragAndDropQuestion(CreateQuestionBankDTO createQuestionBankDDDTO)
        {
            var sortedChoice = createQuestionBankDDDTO.QuizbankAnswers.OrderBy(c => c.Position).ToList();

            createQuestionBankDDDTO.QuizbankAnswers = sortedChoice;

            for (int i = 0; i < sortedChoice.Count; i++)
            {
                int position = i + 1;
                if (sortedChoice[i].Position != position)
                {
                    createQuestionBankDDDTO.Content = createQuestionBankDDDTO.Content.Replace("[[" + sortedChoice[i].Position + "]]", "[[" + position + "]]");
                }
            }
        }

        public void GenerateAnswerTrueFalseQuestion(CreateQuestionBankDTO createQuestionBankDTO)
        {
            CreateQuestionBankAnswerDTO trueAnswer = new CreateQuestionBankAnswerDTO();
            CreateQuestionBankAnswerDTO falseAnswer = new CreateQuestionBankAnswerDTO();
            trueAnswer.Content = "Đúng";
            trueAnswer.Fraction = createQuestionBankDTO.RightAnswer == true ? 1 : 0;
            falseAnswer.Content = "Sai";
            falseAnswer.Fraction = createQuestionBankDTO.RightAnswer == false ? 1 : 0;
            createQuestionBankDTO.QuizbankAnswers = new List<CreateQuestionBankAnswerDTO> { trueAnswer, falseAnswer };
        }

        public async Task<ServiceResponse<CreateQuestionBankDTO>> DeleteQuestionBank(int id)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            var quesResponse = new QuestionBankResponseDTO();

            var qbList = await _dataContext.QuizBanks
                .Include(i => i.QuizbankAnswers)
                .Include(i => i.MatchSubQuestionBanks)
                .Include(i => i.QbTags)
                .Where(c => c.Id == id)
                .ToListAsync();

            foreach (var qb in qbList)
            {
                qb.IsDeleted = 1;
                if (qb.QuizbankAnswers!=null)
                {
                    foreach (var it in qb.QuizbankAnswers) {
                        it.IsDeleted = 1;
                    }
                }
                if (qb.MatchSubQuestionBanks != null)
                {
                    foreach (var it in qb.MatchSubQuestionBanks)
                    {
                        it.IsDeleted = 1;
                    }
                }
                if (qb.QbTags != null)
                {
                    foreach (var it in qb.QbTags)
                    {
                        it.IsDeleted = 1;
                    }
                }
            }
         
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xoá câu hỏi thành công");

            return serviceResponse;
        }
    }
}
