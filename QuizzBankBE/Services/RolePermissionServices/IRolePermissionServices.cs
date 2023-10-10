using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.RolePermissionServices
{
    public interface IRolePermissionServices
    {
        Task<ServiceResponse<RoleDetailPermissionsDTO>> GetDetailRolePermissions(int roleID);
        Task<ServiceResponse<RolePermissionDTO>> UpdatePermissions(List<PermissionDTO> permissionDTOs, int roleID);
    }
}
