using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using System.ComponentModel.DataAnnotations;


public class UniqueValidationAttribute<T> : ValidationAttribute where T : class
{
    private readonly string _methodName;
    private readonly string _propertyName;

    public UniqueValidationAttribute(string propertyName)
    {
        _propertyName = propertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var _dataContext = new DataContext();

        var DbSet = _dataContext.Set<T>();

        var valueExist = DbSet.ToList<T>().Where(
            e =>
            e.GetType().GetProperty(_propertyName).GetValue(e).Equals(value) == true);

        if (valueExist.First() != null)
        {
            return new ValidationResult(_propertyName + "must unique");
        }
        return ValidationResult.Success;
    }
}
