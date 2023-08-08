using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QuizzBankBE.FormValidator
{
    public class ValueEqualWithFieldAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public ValueEqualWithFieldAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherProperty == null)
            {
                throw new ArgumentException("Không tìm thấy trường với tên " + _otherProperty);
            }

            string otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance).ToString();
            if(!value.Equals(otherPropertyValue))
            {
                return new ValidationResult("Mật khẩu xác nhận phải khớp với mật khẩu mới");
            }
            return ValidationResult.Success;
        }
    }
}
