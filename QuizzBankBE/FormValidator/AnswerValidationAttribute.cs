using QuizzBankBE.DTOs;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.FormValidator
{
    public class AnswerValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            float sumFraction = 0;
            var answers = value as ICollection<QuestionBankAnswerDTO>;

            foreach (var answer in answers)
            {
                sumFraction += answer.Fraction;
            }

            if ((int)sumFraction != 1)
            {
                return new ValidationResult("The positive grades you have chosen do not add up to 100%");
            }

            return ValidationResult.Success;
        }
    }
}
