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
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Utility;
using System.Security.Claims;


namespace QuizzBankBE.Services.ListQuestionServices
{
    public class QuestionBankListServicesImpl : IQuestionBankList
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IQuestionServices _questionServices;

        public QuestionBankListServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<PageList<ListQuestionBank>>> GetListQuestionBank(OwnerParameter ownerParameters, int userLoginId, int? categoryId, string? name, string? authorName, string? questionType, string? tag, DateTime? startDate, DateTime? endDate, bool? isPublic)
        {
            var serviceResponse = new ServiceResponse<PageList<ListQuestionBank>>();

            var listTag = tag == null ? new List<string>() : tag.Split(",").
                Select(x => x.TrimStart()).
                ToList();

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            HashSet<string> inputHashet = new HashSet<string>(listTag);

            var dbQuizBanks = await _dataContext.QuizBanks.
                Where(c =>(CheckPermission.IsAdmin(userId) || (c.CreateBy == userLoginId || c.IsPublic == 1)) &&
                        (categoryId == null || c.CategoryId == categoryId) &&
                        (name == null || c.Name.Contains(name)) &&
                        (authorName == null || (c.Author.FirstName + " " + c.Author.LastName).Contains(authorName)) &&
                        (questionType == null || c.QuestionsType == questionType) &&
                        (isPublic == null || c.IsPublic == Convert.ToSByte(isPublic)) &&
                        (startDate == null || endDate == null || (c.CreateDate >= startDate && c.CreateDate <= endDate))).
                Include(q => q.Category).
                Include(q => q.Author).
                Include(q => q.QbTags).
                ThenInclude(q => q.Tag).
                ToListAsync();

            var listAfterSearch = new List<ListQuestionBank>();
            ListQuestionBank listQb = new ListQuestionBank();
            foreach (var item in dbQuizBanks)
            {
                listQb = _mapper.Map<ListQuestionBank>(item);

                listQb.AuthorName = item?.Author?.FirstName + " " + item?.Author?.LastName;
                listQb.CategoryName = item?.Category?.Name;

                var dbTags = item.QbTags.Select(q => q.Tag);

                var listTagName = dbTags.Select(c => c.Name).ToList();

                HashSet<string> listTagNameHashet = new HashSet<string>(listTagName);

                listQb.Tags = _mapper.Map<List<TagDTO>>(dbTags);


                if (inputHashet.IsSubsetOf(listTagNameHashet))
                {
                    listAfterSearch.Add(listQb);
                }
            }

            serviceResponse.Data = PageList<ListQuestionBank>.ToPageList(
            listAfterSearch.AsEnumerable(),

            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

            public async Task<ServiceResponse<PageList<ListQuestion>>> GetListQuestion(OwnerParameter ownerParameters, int userLoginId, string? name, string? authorName, int? categoryId, string? tags, string? questionType, DateTime? startDate, DateTime? endDate)
        {
            var listTag = tags == null ? new List<string>() : tags.Split(",").
               Select(x => x.TrimStart()).
               ToList();
            HashSet<string> inputHashet = new HashSet<string>(listTag);

            var serviceResponse = new ServiceResponse<PageList<ListQuestion>>();
            var dbQuestions = await _dataContext.Questions.
                 Where(c => (c.CreateBy == userLoginId) &&
                        (name == null || c.Name == name) &&
                        (categoryId == null || c.CategoryId == categoryId) &&
                        (authorName == null || (c.Author.FirstName + " " + c.Author.LastName).Contains(authorName)) &&
                        (questionType == null || c.QuestionsType == questionType) &&
                        (startDate == null || endDate == null || (c.CreateDate >= startDate && c.CreateDate <= endDate))).
                Include(q => q.Category).
                Include(q => q.Author).
                Include(q => q.QbTags).
                ThenInclude(q => q.Tag).
                ToListAsync();

            //var questionList = dbQuestions.Select(u => _mapper.Map<ListQuestion>(u)).ToList();
            var listAfterSearch = new List<ListQuestion>();
            ListQuestion listQb = new ListQuestion();

            foreach (var item in dbQuestions)
            {
                listQb = _mapper.Map<ListQuestion>(item);

                listQb.AuthorName = item?.Author?.FirstName + " " + item?.Author?.LastName;
                listQb.CategoryName = item?.Category?.Name;

                var dbTags = item.QbTags.Select(q => q.Tag);

                var listTagName = dbTags.Select(c => c.Name).ToList();

                HashSet<string> listTagNameHashet = new HashSet<string>(listTagName);

                listQb.Tags = _mapper.Map<List<TagDTO>>(dbTags);


                if (inputHashet.IsSubsetOf(listTagNameHashet))
                {
                    listAfterSearch.Add(listQb);
                }
            }

            serviceResponse.Data = PageList<ListQuestion>.ToPageList(
            listAfterSearch.AsEnumerable(),

            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<Boolean>> CreateMultiQuestions(List<int> ids)
        {
            ServiceResponse<Boolean> service = new ServiceResponse<bool>();

            var ques = (from q in _dataContext.QuizBanks
                        join qa in _dataContext.QuizbankAnswers on q.Id equals qa.QuizBankId into qaGroup
                        from qa in qaGroup.DefaultIfEmpty()
                        join mq in _dataContext.MatchSubQuestionBanks on q.Id equals mq.QuestionBankId into mqGroup
                        from mq in mqGroup.DefaultIfEmpty()
                        where ids.Contains(q.Id)
                        select new
                        {
                            Question = q,
                            Answer = qa,
                            MatchAnswer = mq
                        }).GroupBy(i => i.Question).Select(g => new
                        {
                            Question = g.Key,
                            Answers = g.Select(i => i.Answer),
                            MatchAnswers = g.Select(i => i.MatchAnswer)
                        })
                        .ToList();

            foreach(var item in ques)
            {
                Question quesSaved = _mapper.Map<Question>(item.Question);
                quesSaved.Id = 0;
                item.Question.AuthorId = item.Question.CreateBy;

                _dataContext.Questions.Add(quesSaved);
                await _dataContext.SaveChangesAsync();

                List<QuestionAnswer> answerSaved = _mapper.Map<List<QuestionAnswer>>(item.Answers);
                List<MatchSubQuestion>matchSubQuestionsSaved = _mapper.Map<List<MatchSubQuestion>>(item.MatchAnswers);

                if(quesSaved.QuestionsType == "Match")
                {
                    matchSubQuestionsSaved.ForEach(obj => { obj.QuestionId = quesSaved.Id; obj.Id = 0; });
                    _dataContext.MatchSubQuestions.AddRange(matchSubQuestionsSaved);
                }
                else
                {
                    answerSaved.ForEach(obj => { obj.QuestionId = quesSaved.Id; obj.Id = 0; });
                    _dataContext.QuestionAnswers.AddRange(answerSaved);
                }

                await _dataContext.SaveChangesAsync();
            }

            service.Data = true;
            return service;
        }
    }
}
