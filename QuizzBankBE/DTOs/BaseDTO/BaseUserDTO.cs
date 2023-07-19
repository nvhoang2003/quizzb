using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseUserDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string? Firstname { get; set; }

        [Required]
        [StringLength(Const.String)]
        public string? Lastname { get; set; }

        [Required]
        public DateTime? Dob { get; set; }//<

        [Required]
        public string? Address { get; set; }

        [Phone] 
        public string? Phone { get; set; }
        [Required]
        [IdExistValidation<Role>("Id")]
        public int RoleId { get; set; }

        public int? IsDeleted { get; set; }
        
        [Required]
        public int? Gender { get; set; }

        [Required]
        [EmailAddress]
        [UniqueValidation<User>("Email")]
        public string Email { get; set; }
    }
}
