using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using ServiceStack.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class RolePermissionDTO : CreateRolePermissionDTO
    {
        public int Id { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int IsDeleted { get; set; }
    }

    public class CreateRolePermissionDTO
    {
        [Required]
        [IdExistValidation<Role>("Id")]
        public int? RoleId { get; set; }

        [Required]
        [IdExistValidation<Permission>("Id")]
        public int? PermissionId { get; set; }
    }

    public class RoleDetailPermissionsDTO 
    {
        public RoleDTO Role { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }

    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool isPermission { get; set; }
    }
}
