using Microsoft.Extensions.FileSystemGlobbing.Internal;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QuizzBankBE.FormValidator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CheckDragAndDropFields : ValidationAttribute
    {
        private readonly string _otherProperty;

        public CheckDragAndDropFields(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var listChoice = value as List<Choice>;
            var otherProperty = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherProperty == null)
            {
                throw new ArgumentException("Không tìm thấy trường với tên " + _otherProperty);
            }

            string otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance).ToString();
            string pattern = @"\[\[(\d+)\]\]";

            MatchCollection matches = Regex.Matches(otherPropertyValue, pattern);
            foreach (Match match in matches)
            {
                int positionNeedCheck = int.Parse(match.Groups[1].Value);
                bool check = listChoice.Any(c => c.Position == positionNeedCheck);
                if (check == false)
                {
                    return new ValidationResult("Câu trả lời [[" + positionNeedCheck + "]] cần có nội dung.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
