using QuizzBankBE.DTOs.BaseDTO;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateUserDTO : BaseUserDTO
    {
        [Required]public string Username { get; set; }

        [Required]public string Password { get; set; }

     
    }
}
