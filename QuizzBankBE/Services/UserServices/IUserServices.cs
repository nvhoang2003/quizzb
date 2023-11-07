using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.DTOs;
using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.Services.UserServices
{
    public interface IUserServices
    {
        Task<ServiceResponse<PageList<UserDTO>>> GetAllUsers(OwnerParameter ownerParameters, int? courseId, int? roleId);
        Task<ServiceResponse<UserDTO>> GetUserByUserID(int id);
        Task<ServiceResponse<UserDTO>> CreateUsers(CreateUserDTO createUserDTO);
        Task<ServiceResponse<UpdateUserDTO>> UpdateUser(UpdateUserDTO updateDTO, int id);
        Task<ServiceResponse<UpdateUserDTO>> AdminUpdateUser(CreateUserDTO updateDTO, int id);
        Task<ServiceResponse<UpdateUserDTO>> UpdatePassword(UpdatePwdDTO updateDTO);
        Task<ServiceResponse<UpdateUserDTO>> SetActive(int isActive, int id);
    }
}
