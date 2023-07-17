using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseUserDTO
    {
        [Required]
        [StringLength(100)]
        public string? Firstname { get; set; }

        [Required]
        [StringLength(100)]
        public string? Lastname { get; set; }

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
