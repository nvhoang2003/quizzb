using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using System.Linq;

namespace QuizzBankBE.Services.ListQuestionServices
{
    public class QuestionBankListServicesImpl : IQuestionBankList
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        //private readonly IQuestionServices _questionServices;

        public QuestionBankListServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<PageList<ListQuestionBank>>> getListQuestionBank(OwnerParameter ownerParameters, int userLoginId, int? categoryId, string? name, string? authorName, string? questionType, string? tag, DateTime? startDate, DateTime? endDate)
        {
            var serviceResponse = new ServiceResponse<PageList<ListQuestionBank>>();

            var listTag = tag == null ? new List<string>() : tag.Split(", ").
                Select(x => x.TrimStart()).
                ToList();

            HashSet<string> inputHashet = new HashSet<string>(listTag);

            var dbQuizBanks = await _dataContext.QuizBanks.
                Where(c => (c.CreateBy == userLoginId || c.IsPublic == 1) &&
                        (categoryId == null || c.CategoryId == categoryId) &&
                        (name == null || c.Name.Contains(name)) &&
                        (authorName == null || (c.Author.FirstName + " " + c.Author.LastName).Contains(authorName)) &&
                        (questionType == null || c.QuestionsType == questionType) &&
                        (startDate == null || endDate == null || (c.CreateDate >= startDate && c.CreateDate <= endDate))).
                ToListAsync();

            var quizBankList = dbQuizBanks.Select(u => _mapper.Map<ListQuestionBank>(u)).ToList();

            var listAfterSearch = new List<ListQuestionBank>();

            foreach (var item in quizBankList)
            {
                var quizBank = dbQuizBanks.First(c => c.Id == item.Id);
                item.AuthorName = await _dataContext.Users.Where(c => c.Id == quizBank.CreateBy).Select(c => c.FirstName + " " + c.LastName).FirstOrDefaultAsync();

                item.CategoryName = await _dataContext.Categories.Where(c => c.Id == quizBank.CategoryId).Select(c => c.Name).FirstOrDefaultAsync();

                var dbTags = (from q in _dataContext.QuizBanks
                                join qt in _dataContext.QbTags on q.Id equals qt.QbId
                                join t in _dataContext.Tags on qt.TagId equals t.Id
                                where q.Id == item.Id
                                select t).Distinct().ToList();

                var listTagName = dbTags.Select(c => c.Name).ToList();

                HashSet<string> listTagNameHashet = new HashSet<string>(listTagName);

                item.Tags = _mapper.Map<List<TagDTO>>(dbTags);


                if (inputHashet.IsSubsetOf(listTagNameHashet))
                {
                    listAfterSearch.Add(item);
                }
            }

            serviceResponse.Data = PageList<ListQuestionBank>.ToPageList(
            listAfterSearch.AsEnumerable(),

            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

            public async Task<ServiceResponse<PageList<ListQuestion>>> getListQuestion(OwnerParameter ownerParameters, int userLoginId, string? name, string? authorName, string? questionType, DateTime? startDate, DateTime? endDate)
        {
            var serviceResponse = new ServiceResponse<PageList<ListQuestion>>();
            var dbQuestions = await _dataContext.Questions.
                 Where(c => (c.CreateBy == userLoginId) &&
                        (name == null || c.Name == name) &&
                        (authorName == null || (c.Author.FirstName + " " + c.Author.LastName).Contains(authorName)) &&
                        (questionType == null || c.QuestionsType == questionType) &&
                        (startDate == null || endDate == null || (c.CreateDate >= startDate && c.CreateDate <= endDate))).
                ToListAsync();

            var questionList = dbQuestions.Select(u => _mapper.Map<ListQuestion>(u)).ToList();

            foreach (var item in questionList)
            {
                var quizBank = dbQuestions.First(c => c.Id == item.Id);
                var author = await _dataContext.Users.FirstOrDefaultAsync(c => c.Id == quizBank.AuthorId);
                item.AuthorName = author.FirstName + author.LastName;
            }

            serviceResponse.Data = PageList<ListQuestion>.ToPageList(
            questionList.AsEnumerable(),

            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<Boolean>> createMultiQuestions(List<int> ids, int authorId)
        {
            ServiceResponse<Boolean> service = new ServiceResponse<bool>();

            var ques = (from q in _dataContext.QuizBanks
                        join qa in _dataContext.QuizbankAnswers on q.Id equals qa.QuizBankId
                        where ids.Contains(q.Id)
                        select new
                        {
                            Question = q,
                            Answer = qa
                        }).GroupBy(i => i.Question).Select(g => new
                        {
                            Question = g.Key,
                            Answers = g.Select(i => i.Answer)
                        })
                        .ToList();

            foreach(var item in ques)
            {
                Question quesSaved = _mapper.Map<Question>(item.Question);
                quesSaved.Id = 0;
                item.Question.AuthorId = authorId;
                _dataContext.Questions.Add(quesSaved);
                await _dataContext.SaveChangesAsync();
                List<QuestionAnswer> answerSaved = _mapper.Map<List<QuestionAnswer>>(item.Answers);
                answerSaved.ForEach(obj => { obj.QuestionId = quesSaved.Id; obj.Id = 0; });
                _dataContext.QuestionAnswers.AddRange(answerSaved);
                await _dataContext.SaveChangesAsync();
            }

            service.Data = true;
            return service;
        }
    }
}
