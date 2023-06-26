using QuizzBankBE.DTOs.BaseDTO;

namespace QuizzBankBE.DTOs
{
    public class UserDTO : BaseUserDTO
    {
        public int Iduser { get; set; }

        public string Username { get; set; }
    }
}
