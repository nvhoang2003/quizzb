using AutoMapper;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Services.ListQuestionServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class QuestionBankValidateServiceImpl : IQuestionBankValidate
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public QuestionBankValidateServiceImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<Dictionary<string, string>>> CheckValidate(CreateQuestionBankDTO createQuestionBankDTO)
        {
            ServiceResponse<Dictionary<string, string>> response = new ServiceResponse<Dictionary<string, string>>();

            string nameFunctionValidate = "Check" + createQuestionBankDTO.Questionstype;
            QuestionBankValidateServiceImpl myClassInstance = new QuestionBankValidateServiceImpl(_dataContext, _mapper, _configuration, _jwtProvider);

            MethodInfo methodInfo = typeof(QuestionBankValidateServiceImpl).GetMethod(nameFunctionValidate);

            object[] parameters = new object[] { createQuestionBankDTO };

            // Gọi phương thức 'MyMethod' một cách động
            var result = methodInfo.Invoke(myClassInstance, parameters);

            Dictionary<string, string> myDict = result as Dictionary<string, string>;
            response.Data = myDict;

            if(myDict.Count > 0)
            {
                response.Status = false;
                response.StatusCode = 400;
            }

            return response;
        }

        public Dictionary<string, string> CheckMultiChoice(CreateQuestionBankDTO createQuestionBankDTO)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if(createQuestionBankDTO.QuizbankAnswers == null)
            {
                errors.Add("Answer", "Vui lòng nhập câu trả lời");
            }

            if(createQuestionBankDTO.QuizbankAnswers != null && createQuestionBankDTO.QuizbankAnswers.Select(q  => q.Fraction).Sum() != 1)
            {
                errors.Add("Answer", "Tổng điểm phải luôn là 100%");
            }

            return errors;
        }

        public Dictionary<string, string> CheckTrueFalse(CreateQuestionBankDTO createQuestionBankDTO)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (createQuestionBankDTO.RightAnswer == null)
            {
                errors.Add("RightAnswer", "Vui lòng chọn đáp án đúng");
            }

            return errors;
        }

        public Dictionary<string, string> CheckMatch(CreateQuestionBankDTO createQuestionBankDTO)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (createQuestionBankDTO.MatchSubQuestionBanks == null || createQuestionBankDTO.MatchSubQuestionBanks.Count < 3 || createQuestionBankDTO.MatchSubQuestionBanks.Where(q => !string.IsNullOrEmpty(q.AnswerText)).Count() < 2)
            {
                errors.Add("MatchSubQuestionBanks", "Vui lòng nhập ít nhất 2 câu hỏi phụ và 3 câu trả lời");
            }
            else
            {
                foreach(var item in createQuestionBankDTO.MatchSubQuestionBanks.Select((value, index) => new { value, index }))
                {
                    if (item.value.AnswerText.IsNullOrEmpty())
                    {
                        errors.Add($"MatchSubQuestionBanks[{item.index}]", "Vui lòng nhập câu trả lời");
                    }
                }
            }
         

            return errors;
        }

        public Dictionary<string, string> CheckShortAnswer(CreateQuestionBankDTO createQuestionBankDTO)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (createQuestionBankDTO.QuizbankAnswers == null)
            {
                errors.Add("Answer", "Vui lòng nhập đáp án đúng");
            }
            else 
            {
                if(createQuestionBankDTO.QuizbankAnswers.Any(q => q.Fraction > 1))
                {
                    errors.Add("Answer", "Đáp án của bạn điểm không được quá 100%");
                }
                if(createQuestionBankDTO.QuizbankAnswers.Where(q => q.Fraction ==1).Count() == 0)
                {
                    errors.Add("Answer", "Phải có ít nhất 1 đáp án điểm là 100%");
                }
            }

            return errors;
        }

        public Dictionary<string, string> CheckDragAndDropIntoText(CreateQuestionBankDTO createQuestionBankDTO)
        {
            Regex regex = new Regex(@".*\[\[\d+\]\].*");
            var listChoice = createQuestionBankDTO.QuizbankAnswers?.Select(q => q.Position).ToList();

            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (regex.IsMatch(createQuestionBankDTO.Content) == false)
            {
                errors.Add("content", "Nội dung câu hỏi phải chứa [[x]] với x là 1 số bất kì");
            }

            if (createQuestionBankDTO.QuizbankAnswers == null)
            {
                errors.Add("Answer", "Vui lòng nhập các lựa chọn");
            }
            else
            {
                string pattern = @"\[\[(\d+)\]\]";

                MatchCollection matches = Regex.Matches(createQuestionBankDTO.Content, pattern);
                foreach (Match match in matches)
                {
                    int positionNeedCheck = int.Parse(match.Groups[1].Value);
                    bool check = listChoice.Any(c => c == positionNeedCheck);
                    if (check == false)
                    {
                        errors.Add("Choice", $"Lựa chọn [[{positionNeedCheck}]] cần có nội dung");
                    }
                }
            }
            
            return errors;
        }
    }
}
