using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class UserDTO : BaseUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FullName
        {
            get { return Firstname + " " + Lastname; }
        }
    }

    public class UpdateUserDTO : BaseUserDTO {

        private DataContext _dataContext = new DataContext();

        [Required]
        [UniqueValidation<User>("GetDbSet", "UserName")]
        public string UserName { get; set; }
        public IEnumerable<User> GetDbSet()
        {
            return _dataContext.Set<User>();
        }
    }
}
