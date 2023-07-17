
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    
    public class CreateUserDTO : BaseUserDTO
    {
        private DataContext _dataContext = new DataContext();

        [Required]
        [UniqueValidation<User>("GetDbSet", "UserName")]
        public string UserName { get; set; }
        public IEnumerable<User> GetDbSet()
        {
            return _dataContext.Set<User>();
        }

        [Required]
        //[DataType(DataType.Password)]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
        //    ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        //Mật khẩu phải có ít nhất 8 ký tự và chứa ít nhất 3 trong 4 ký tự sau: chữ hoa (AZ), chữ thường (az), số (0 -9) và ký tự đặc biệt (ví dụ !@#$%^&*)")

        public string Password { get; set; }


    }
}
