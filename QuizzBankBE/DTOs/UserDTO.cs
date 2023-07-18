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
        [Required]
        [UniqueValidation<User>("UserName")]
        public string UserName { get; set; }
    }
}
