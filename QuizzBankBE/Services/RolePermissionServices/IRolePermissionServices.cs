using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.RolePermissionServices
{
    public interface IRolePermissionServices
    {
        Task<ServiceResponse<RoleDetailPermissionsDTO>> getDetailRolePermissions(int roleID);
        Task<ServiceResponse<RolePermissionDTO>> updatePermissions(List<PermissionDTO> permissionDTOs, int roleID);
    }
}
