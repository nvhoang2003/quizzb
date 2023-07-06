using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseUserDTO
    {
        [Required]
        [StringLength(255)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string? LastName { get; set; }
        [Required]
        public DateTime? Dob { get; set; }//<
        [Required]
        public string? Address { get; set; }

        [Phone] 
        public string? Phone { get; set; }
        public int? IsDeleted { get; set; }
        [Required]
        public int? Gender { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }
    }
}
