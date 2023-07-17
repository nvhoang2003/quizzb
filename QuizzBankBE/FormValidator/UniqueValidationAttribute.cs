using QuizzBankBE.DataAccessLayer.DataObject;
using System.ComponentModel.DataAnnotations;


public class UniqueValidationAttribute<T> : ValidationAttribute
{
    private readonly string _methodName;
    private readonly string _propertyName;

    public UniqueValidationAttribute(string methodName, string propertyName)
    {
        _methodName = methodName;
        _propertyName = propertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();

        var method = type.GetMethod(_methodName);

        var dbSet = (IEnumerable<T>)method.Invoke(instance, null);

        var existingValue = dbSet.FirstOrDefault(e =>
            !Equals(e, validationContext.ObjectInstance) &&
            e.GetType().GetProperty(_propertyName)?.GetValue(e)?.Equals(value) == true);

        if (existingValue != null)
        {
            return new ValidationResult(_propertyName + " must uniq");
        }
        return ValidationResult.Success;
    }
}
