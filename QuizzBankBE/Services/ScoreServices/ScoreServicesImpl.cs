using AutoMapper;
using Newtonsoft.Json;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using System.Text.Json;
using System.Reflection;
using QuizzBankBE.Services.QuizService;

namespace QuizzBankBE.Services.ScoreServices
{
    public class ScoreServicesImpl : IScoreServicesImpl
    {
        public static DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        public IQuizService _quizService;

        public ScoreServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _quizService = new QuizServiceIpml(dataContext, mapper, configuration, jwtProvider);
        }

        public ScoreServicesImpl()
        {
        }

        public async Task<ServiceResponse<ResultQuizDTO>> DoQuestion(QuizSubmmitDTO newQuizResponses)
        {
            var servicesResponse = new ServiceResponse<ResultQuizDTO>();
            var resultAfterSubmitQuiz = new ResultQuizDTO();
            var quizForTest = await _quizService.ShowQuizForTest(newQuizResponses.QuizId, "");
            var data = quizForTest.Data;
            var saveQuizResponse = new List<QuizResponse>();

            foreach(var item in newQuizResponses.listQuestionSubmit)
            {
                if(data.questionReults.Any(qr => qr.Id == item.QuestionId && qr.QuestionsType == item.QuestionType))
                {
                    string nameFunction= "Check" + item.QuestionType;
                    ScoreServicesImpl myClassInstance = new ScoreServicesImpl(_dataContext, _mapper, _configuration, _jwtProvider);

                    MethodInfo methodInfo = typeof(ScoreServicesImpl).GetMethod(nameFunction);

                    object[] parameters = new object[] { item, data, saveQuizResponse };

                    var result = methodInfo.Invoke(myClassInstance, parameters);

                    Tuple<bool, List<QuizResponse>> res = result as Tuple<bool, List<QuizResponse>>;

                    if (res.Item1 == false)
                    {
                        servicesResponse.Status = false;
                        servicesResponse.StatusCode = 400;
                        servicesResponse.Message = $"Câu hỏi {newQuizResponses.listQuestionSubmit.IndexOf(item) + 1} đưa câu trả lời sai định dạng";
                        return servicesResponse;
                    }

                    saveQuizResponse = res.Item2;
                }
                else
                {
                    servicesResponse.Status = false;
                    servicesResponse.StatusCode = 400;
                    servicesResponse.Message = $"Câu hỏi {newQuizResponses.listQuestionSubmit.IndexOf(item) + 1 } không tồn tại trong đề";
                    return servicesResponse;
                }
            }
            saveQuizResponse.ForEach(q => q.AccessId = newQuizResponses.AccessId);
            _dataContext.QuizResponses.AddRange(saveQuizResponse);
            var dbQuizAccess = _dataContext.QuizAccesses.Where(q => q.Id == newQuizResponses.AccessId).First();

            dbQuizAccess.Status = "Done";
            _dataContext.QuizAccesses.Update(dbQuizAccess);

            await _dataContext.SaveChangesAsync();

            resultAfterSubmitQuiz.Quiz = data.quiz;
            resultAfterSubmitQuiz.UserName = data.userName;
            resultAfterSubmitQuiz.CourseName = data.courseName;
            resultAfterSubmitQuiz.Point = saveQuizResponse.Select(sqr => sqr.Mark).Sum();
            resultAfterSubmitQuiz.Status = saveQuizResponse.Select(sqr => sqr.Mark).Sum() > data.quiz.PointToPass ? "Qua Môn" : "Thi Lại";

            servicesResponse.Data = resultAfterSubmitQuiz;
            return servicesResponse;
        }

        public Tuple<bool, List<QuizResponse>?> CheckMultiChoice(OneQuestionSubmitDTO? item, QuizResponseForTest? data, List<QuizResponse>? quizResponses)
        {
            Dictionary<bool, List<QuizResponse>> result = new Dictionary<bool, List<QuizResponse>>();
            float? point = 0;

            if (item?.IdAnswerChoosen == null || item.IdAnswerChoosen.Count == 0)
            {
                return Tuple.Create(false, quizResponses);
            }

            var question = data.questionReults.Where(qr => qr.Id == item.QuestionId).First();
            bool isRight = true;
            float? sumFRaction = 0;
            string status = "Wrong";

            foreach(var oneAnswer in item.IdAnswerChoosen)
            {
                if(question.QuestionAnswers.Where(qa => qa.Id == oneAnswer).First().Fraction == 0)
                {
                    isRight = false;
                    break;
                }
                sumFRaction += question.QuestionAnswers.Where(qa => qa.Id == oneAnswer).First().Fraction;
            }

            if(sumFRaction == 1.0 && isRight == true)
            {
                point = data.questionReults.Where(qr => qr.Id == item.QuestionId).First().DefaultMark;
                status = "Right";
            }

            var quizResponse = new QuizResponse();
            quizResponse.QuestionId = item.QuestionId;
            quizResponse.Mark = point;
            quizResponse.Status = status;
            quizResponse.Answer = JsonConvert.SerializeObject(item.IdAnswerChoosen);

            quizResponses.Add(quizResponse);

            return Tuple.Create(true, quizResponses);
        }

        public Tuple<bool, List<QuizResponse>?> CheckMatch(OneQuestionSubmitDTO? item, QuizResponseForTest? data, List<QuizResponse>? quizResponses)
        {
            Dictionary<bool, List<QuizResponse>> result = new Dictionary<bool, List<QuizResponse>>();
            if (item?.MatchSubQuestionChoosen == null || item.MatchSubQuestionChoosen.Count == 0)
            {
                return Tuple.Create(false, quizResponses);
            }

            float? point = 0;            
            var question = data.questionReults.Where(qr => qr.Id == item.QuestionId).First();
            float? oneMatchSubPoint = question.DefaultMark / question.MatchSubQuestions.Where(q => q.QuestionText != "" && q.QuestionText != null).Count();
            bool isRight = true;

            foreach(var oneMatchSub in item.MatchSubQuestionChoosen)
            {
                point += question.MatchSubQuestions.Any(q => q.QuestionText?.Trim() == oneMatchSub.QuestionText.Trim() && q.AnswerText?.Trim() == oneMatchSub.AnswerText.Trim())? oneMatchSubPoint : 0;
            }

            var quizResponse = new QuizResponse();
            quizResponse.QuestionId = item.QuestionId;
            quizResponse.Mark = point;
            quizResponse.Status = point == question.DefaultMark ? "Right" : "Wrong";
            quizResponse.Answer = JsonConvert.SerializeObject(item.MatchSubQuestionChoosen);

            quizResponses.Add(quizResponse);

            return Tuple.Create(true, quizResponses);
        }

        public Tuple<bool, List<QuizResponse>?> CheckShortAnswer(OneQuestionSubmitDTO? item, QuizResponseForTest? data, List<QuizResponse>? quizResponses)
        {
            Dictionary<bool, List<QuizResponse>> result = new Dictionary<bool, List<QuizResponse>>();
            if (item?.ShortAnswerChoosen == null || item.ShortAnswerChoosen == "")
            {
                return Tuple.Create(false, quizResponses);
            }

            var question = data.questionReults.Where(qr => qr.Id == item.QuestionId).First();
            string status = "Right";
            float? point = question.QuestionAnswers.Where(q => q.Content == item.ShortAnswerChoosen).FirstOrDefault()?.Fraction;

            if (point < 1)
            {
                status = "Wrong";
            }

            var quizResponse = new QuizResponse();
            quizResponse.QuestionId = item.QuestionId;
            quizResponse.Mark = point * question.DefaultMark;
            quizResponse.Status = status;
            quizResponse.Answer = JsonConvert.SerializeObject(item.ShortAnswerChoosen);

            quizResponses.Add(quizResponse);

            return Tuple.Create(true, quizResponses);
        }

        public Tuple<bool, List<QuizResponse>?> CheckTrueFalse(OneQuestionSubmitDTO? item, QuizResponseForTest? data, List<QuizResponse>? quizResponses)
        {
            Dictionary<bool, List<QuizResponse>> result = new Dictionary<bool, List<QuizResponse>>();
            float? point = 0;

            if (item?.IdAnswerChoosen == null || item.IdAnswerChoosen.Count != 1)
            {
                return Tuple.Create(false, quizResponses);
            }

            var question = data.questionReults.Where(qr => qr.Id == item.QuestionId).First();
            bool isRight = true;
            float? sumFRaction = 0;
            string status = "Wrong";

            foreach (var oneAnswer in item.IdAnswerChoosen)
            {
                if (question.QuestionAnswers.Where(qa => qa.Id == oneAnswer).First().Fraction == 0)
                {
                    isRight = false;
                    break;
                }
                sumFRaction += question.QuestionAnswers.Where(qa => qa.Id == oneAnswer).First().Fraction;
            }

            if (sumFRaction == 1.0 && isRight == true)
            {
                point = data.questionReults.Where(qr => qr.Id == item.QuestionId).First().DefaultMark;
                status = "Right";
            }

            var quizResponse = new QuizResponse();
            quizResponse.QuestionId = item.QuestionId;
            quizResponse.Mark = point;
            quizResponse.Status = status;
            quizResponse.Answer = JsonConvert.SerializeObject(item.IdAnswerChoosen);

            quizResponses.Add(quizResponse);

            return Tuple.Create(true, quizResponses);
        }

    }
}
