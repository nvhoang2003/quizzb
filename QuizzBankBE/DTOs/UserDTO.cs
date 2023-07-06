using QuizzBankBE.DTOs.BaseDTO;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class UserDTO : BaseUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

    }
}
