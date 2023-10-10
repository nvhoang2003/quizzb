using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DataAccessLayer.DataObject;
using Microsoft.AspNetCore.SignalR;

namespace QuizzBankBE.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserServices(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            _dataContext = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<PageList<UserDTO>>> GetAllUsers(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<UserDTO>>();
            var userDTOs = new List<UserDTO>();
            var dbUsers = await _dataContext.Users.ToListAsync();

            userDTOs = dbUsers.Select(u => _mapper.Map<UserDTO>(u)).ToList();

            serviceResponse.Data = PageList<UserDTO>.ToPageList(
            userDTOs.AsEnumerable<UserDTO>(),//.OrderBy(on => on.Email)
            ownerParameters.pageIndex,
            ownerParameters.pageSize);
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserDTO>> GetUserByUserID(int id )
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            var dbUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);

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

        public async Task<ServiceResponse<UserDTO>> CreateUsers(CreateUserDTO createUserDTO)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();

            createUserDTO.Password = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password);
            User userRegister = _mapper.Map<User>(createUserDTO);
            _dataContext.Users.Add(userRegister);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<UserDTO>(userRegister);
            serviceResponse.Status = true;
            serviceResponse.StatusCode = 200;
            serviceResponse.Message = "Tạo thành công !";

            return serviceResponse;
        }

        public async Task<ServiceResponse<UpdateUserDTO>> UpdateUser(UpdateUserDTO updateDTO, int id)
        {
            var serviceResponse = new ServiceResponse<UpdateUserDTO>();
            var dbUser = await _dataContext.Users.FirstOrDefaultAsync(q => q.Id == id);
            if (dbUser == null)
            {
                serviceResponse.updateResponse(400, "User không tồn tại");
                return serviceResponse;
            }

            dbUser.UserName= updateDTO.UserName;
            dbUser.FirstName = updateDTO.Firstname;
            dbUser.LastName = updateDTO.Lastname;
            dbUser.Dob = updateDTO.Dob;
            dbUser.Gender= updateDTO.Gender;
            dbUser.Email = updateDTO.Email;
            dbUser.Address = updateDTO.Address;
            dbUser.Phone= updateDTO.Phone;

            _dataContext.Users.Update(dbUser);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Update thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<UpdateUserDTO>> UpdatePassword(UpdatePwdDTO updateDTO)
        {
            var serviceResponse = new ServiceResponse<UpdateUserDTO>();

            var user = await _dataContext.Users.FirstOrDefaultAsync(c => c.Id == updateDTO.UserId);
            if(VerifyPassword(updateDTO.OldPwd, user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateDTO.NewPwd);
                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();
                serviceResponse.updateResponse(200, "Update thành công");
            }
            else
            {
                serviceResponse.updateResponse(400, "Sai mật khẩu");
            };

            return serviceResponse;
        }

        private bool VerifyPassword(string password, string storedpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedpassword);
        }

        public async Task<ServiceResponse<UpdateUserDTO>> AdminUpdateUser(CreateUserDTO updateDTO, int id)
        {

            var serviceResponse = new ServiceResponse<UpdateUserDTO>();
            var dbUser = await _dataContext.Users.FirstOrDefaultAsync(q => q.Id == id);
            if (dbUser == null)
            {
                serviceResponse.updateResponse(400, "User không tồn tại");
                return serviceResponse;
            }

            dbUser.UserName = updateDTO.UserName;
            dbUser.FirstName = updateDTO.Firstname;
            dbUser.LastName = updateDTO.Lastname;
            dbUser.Dob = updateDTO.Dob;
            dbUser.Gender = updateDTO.Gender;
            dbUser.Email = updateDTO.Email;
            dbUser.Address = updateDTO.Address;
            dbUser.Phone = updateDTO.Phone;
            dbUser.Password = BCrypt.Net.BCrypt.HashPassword(updateDTO.Password);

            _dataContext.Users.Update(dbUser);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Update thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<UpdateUserDTO>> SetActive(int isActive, int id)
        {
            var serviceResponse = new ServiceResponse<UpdateUserDTO>();
            var dbUser = await _dataContext.Users.FirstOrDefaultAsync(q => q.Id == id);
            dbUser.IsActive = (sbyte?)isActive;

            _dataContext.Users.Update(dbUser);
            await _dataContext.SaveChangesAsync();
            return serviceResponse;
        }
    }
}
