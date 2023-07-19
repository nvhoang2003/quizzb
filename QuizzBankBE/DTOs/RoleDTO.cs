using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Description { get; set; }

        public int? CreateBy { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int IsDeleted { get; set; }
    }

    public class CreateRoleDTO {

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Description { get; set; }

    }
    public class RoleResponseDTO
    {
        public string Status { get; set; }

        public int Version { get; set; }

        public int? IsDeleted { get; set; }
    }

}
