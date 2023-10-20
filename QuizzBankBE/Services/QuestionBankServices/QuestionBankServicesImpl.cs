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

            var ques = (from q in _dataContext.QuizBanks
                        join qa in _dataContext.QuizbankAnswers on q.Id equals qa.QuizBankId into qaGroup
                        from qa in qaGroup.DefaultIfEmpty()
                        join mq in _dataContext.MatchSubQuestionBanks on q.Id equals mq.QuestionBankId into mqGroup
                        from mq in mqGroup.DefaultIfEmpty()
                        join qt in _dataContext.QbTags on q.Id equals qt.QbId into qtGroup
                        from qt in qtGroup.DefaultIfEmpty()
                        join t in _dataContext.Tags on qt.TagId equals t.Id into tGroup
                        from t in tGroup.DefaultIfEmpty()
                        where q.Id == id
                        select new
                        {
                            Question = q,
                            Answer = qa,
                            MatchAnswer = mq,
                            Tag = t
                        }).GroupBy(i => i.Question).Select(g => new
                        {
                            Question = g.Key,
                            Answers = g.Select(i => i.Answer),
                            MatchAnswers = g.Select(i => i.MatchAnswer),
                            Tags = g.Select(i => i.Tag)
                        })
                        .FirstOrDefault();

            if(quesResponse == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            quesResponse = _mapper.Map<QuestionBankResponseDTO>(ques.Question);
            quesResponse.QuizbankAnswers = _mapper.Map<List<CreateQuestionBankAnswerDTO>>(ques.Answers.Distinct());
            quesResponse.MatchSubQuestionBanks = _mapper.Map<List<CreateMatchSubQuestionBank>>(ques.MatchAnswers.Distinct());
            quesResponse.Tags = _mapper.Map<List<TagDTO>>(ques.Tags.Distinct());

            serviceResponse.Data = quesResponse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<CreateQuestionBankDTO>> UpdateQuestionBank(int questionBankID, CreateQuestionBankDTO updateQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == questionBankID);
            _mapper.Map(updateQuestionBankDTO, quesToUpdate);

            if (quesToUpdate == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            if (updateQuestionBankDTO.Questionstype == "TrueFalse")
            {
                GenerateAnswerTrueFalseQuestion(updateQuestionBankDTO);
            }
            if (updateQuestionBankDTO.Questionstype == "DragAndDropIntoText")
            {
                UpdateContentDragAndDropQuestion(updateQuestionBankDTO);
            }

            await _dataContext.SaveChangesAsync();

            await _qestionBanlListService.CreateMultiQuestions(new List<int> { questionBankID });


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
            CreateQuestionBankAnswerDTO rightAnswer = new CreateQuestionBankAnswerDTO();
            CreateQuestionBankAnswerDTO wrongtAnswer = new CreateQuestionBankAnswerDTO();
            rightAnswer.Content = createQuestionBankDTO?.RightAnswer?.ToString();
            rightAnswer.Fraction = 1;
            wrongtAnswer.Content = createQuestionBankDTO.RightAnswer == true ? "False" : "True";
            wrongtAnswer.Fraction = 0;
            createQuestionBankDTO.QuizbankAnswers = new List<CreateQuestionBankAnswerDTO> { rightAnswer, wrongtAnswer };
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

            if (qbList.Count == 0)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

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
