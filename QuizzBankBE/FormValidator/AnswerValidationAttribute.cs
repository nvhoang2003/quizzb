using QuizzBankBE.DTOs;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.FormValidator
{
    public class AnswerValidationAttribute<T> : ValidationAttribute where T : class
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            float sumFraction = 0;
            var answers = value as ICollection<T>;
            foreach (var answer in answers)
            {
                sumFraction += (float)answer.GetType().GetProperty("Fraction").GetValue(answer);
            }
            if ((int)sumFraction != 1)
            {
                return new ValidationResult("The positive grades you have chosen do not add up to 100%");
            }

            return ValidationResult.Success;
        }
    }
}
