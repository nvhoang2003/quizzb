using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
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

    public class UpdatePwdDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string OldPwd { get; set; }

        [Required]
        public string NewPwd { get; set; }
        
        [ValueEqualWithField("NewPwd")]
        [Required]
        public string ConfirmPwd { get; set; }
    }
}
