using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.DTOs;
using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.Services.UserServices
{
    public interface IUserServices
    {
        Task<ServiceResponse<PageList<UserDTO>>> getAllUsers(OwnerParameter ownerParameters);
        Task<ServiceResponse<UserDTO>> getUserByUserID(int id);
        Task<ServiceResponse<UserDTO>> createUsers(CreateUserDTO createUserDTO);
        Task<ServiceResponse<UpdateUserDTO>> updateUser(UpdateUserDTO updateDTO, int id);
        
    }
}
