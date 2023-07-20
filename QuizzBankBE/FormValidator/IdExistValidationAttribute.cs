using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using System.Data;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.FormValidator
{
    public class IdExistValidationAttribute<T> : ValidationAttribute where T : class
    {
        private readonly string _columnName;
        public IdExistValidationAttribute(string columnName)
        {
            _columnName = columnName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _dataContext = new DataContext();

            var DbSet = _dataContext.Set<T>();

            var valueExist = DbSet.ToList<T>().Where(
                e =>
                e.GetType().GetProperty(_columnName).GetValue(e).Equals(value) == true);

            if (valueExist.FirstOrDefault() == null)
            {
                return new ValidationResult(_columnName + " does not exist");
            }
            return ValidationResult.Success;
        }
    }
}