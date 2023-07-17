using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model;
using AutoMapper;
using System.Security.Principal;
using System.Security.Claims;
using QuizzBankBE.JWT;

namespace QuizzBankBE.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public AuthServices(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public AuthServices()
        {
        }

        private bool VerifyPassword(string password, string storedpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedpassword);
        }

        public async Task<IPrincipal> createPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id + "")
            };
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

        public async Task<LoginResponse> login(LoginForm loginForm)
        {
            var serviceResponse = new LoginResponse();
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == loginForm.username);
            if (user != null && VerifyPassword(loginForm.password, user.Password))
            {
                serviceResponse.Status = true;
                serviceResponse.StatusCode = 200;
                IPrincipal userlogin = await createPrincipal(user);
                var token = _jwtProvider.CreateToken(userlogin);
                serviceResponse.Message = "Đăng nhập thành công!";
                serviceResponse.accessToken = token;
                return serviceResponse;
            }
            else
            {
                serviceResponse.Status = false;
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Tài khoản hoặc mật khẩu không chính xác";
                return serviceResponse;
            }
        }
    }
}
