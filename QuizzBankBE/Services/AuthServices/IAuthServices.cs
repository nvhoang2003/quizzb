using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model;
using QuizzBankBE.DTOs;

namespace QuizzBankBE.Services.AuthServices
{
    public interface IAuthServices
    {
        Task<LoginResponse> login(LoginForm loginFrom);
    }
}
