using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.RoleServices
{
    public interface IRoleServices
    {
        Task<ServiceResponse<RoleResponseDTO>> CreateNewRole(CreateRoleDTO createRoleDTO);
        Task<ServiceResponse<RoleDTO>> GetRoleByID(int id);
        Task<ServiceResponse<PageList<RoleDTO>>> GetAllRole(OwnerParameter ownerParameters);
        Task<ServiceResponse<RoleDTO>> DeleteRole(int id);
        Task<ServiceResponse<RoleDTO>> UpdateRole(CreateRoleDTO updateRoleDTO, int id);
    }
}
