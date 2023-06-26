using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseUserDTO
    {
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public DateTime? Dob { get; set; }

        public string? Address { get; set; }

        [Phone] public string? Phone { get; set; }

        public int? Gender { get; set; }

        [Required][EmailAddress] public string Email { get; set; }
    }
}
