using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.RoleServices
{
    public interface IRoleServices
    {
        Task<ServiceResponse<RoleResponseDTO>> createNewRole(CreateRoleDTO createRoleDTO);

        Task<ServiceResponse<PageList<RoleDTO>>> getAllRole(OwnerParameter ownerParameters);
        Task<ServiceResponse<RoleDTO>> deleteRole(int id);
        Task<ServiceResponse<RoleDTO>> updateRole(CreateRoleDTO updateRoleDTO, int id);
    }
}
