using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;

namespace QuizzBankBE.Services.UserServices
{
    public class UserServices :IUserServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserServices(DataContext context, IMapper mapper, IConfiguration configuration) {
            _dataContext = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<PageList<UserDTO>>> getAllUsers(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<UserDTO>>();
            var userDTOs = new List<UserDTO>();
            var dbUsers = await _dataContext.Users.ToListAsync();
            userDTOs = dbUsers.Select(u => _mapper.Map<UserDTO>(u)).ToList();
            serviceResponse.Data = PageList<UserDTO>.ToPageList(
            userDTOs.AsEnumerable<UserDTO>().OrderBy(on => on.Email),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserDTO>> getUserByUserID(int userID)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            var dbUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Iduser == userID);

            if (dbUser == null)
            {
                serviceResponse.Status = false;
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "user.notFoundwithID";
                return serviceResponse;
            }

            serviceResponse.Data = _mapper.Map<UserDTO>(dbUser);

            return serviceResponse;
        }
    }
}
