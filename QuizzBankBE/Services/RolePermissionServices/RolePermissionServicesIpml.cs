using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using System.Linq;

namespace QuizzBankBE.Services.RolePermissionServices
{
    public class RolePermissionServicesIpml : IRolePermissionServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolePermissionServicesIpml(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<RoleDetailPermissionsDTO>> GetDetailRolePermissions(int roleID)
        {
            var serviceResponse = new ServiceResponse<RoleDetailPermissionsDTO>();
            var rolePersResDto = new RoleDetailPermissionsDTO();

            var role = await _dataContext.Roles.FirstOrDefaultAsync(e => e.Id.Equals(roleID));

            if (role == null)
            {
                serviceResponse.updateResponse(404, "Không tìm thấy!");

                return serviceResponse;
            }

            var permissions = await _dataContext.Permissions.ToListAsync();
            var permissionsDto = _mapper.Map<List<PermissionDTO>>(permissions);
            var permissionsOfRole = await _dataContext.RolePermissions.Where(e => e.RoleId.Equals(roleID)).ToListAsync();

            foreach (PermissionDTO permissionDto in permissionsDto)
            {
                if (permissionsOfRole.FirstOrDefault(e => e.PermissionId.Equals(permissionDto.Id)) != null)
                {
                    permissionDto.isPermission = true;
                }
            }

            rolePersResDto = _mapper.Map<RoleDetailPermissionsDTO>(role);
            rolePersResDto.Permissions = permissionsDto;

            serviceResponse.Data = rolePersResDto;
            serviceResponse.Message = "OK";

            return serviceResponse;
        }

        public async Task<ServiceResponse<RolePermissionDTO>> UpdatePermissions(List<PermissionDTO> permissionDTOs, int roleID)
        {
            var serviceResponse = new ServiceResponse<RolePermissionDTO>();

            var role = await _dataContext.Roles.FirstOrDefaultAsync(e => e.Id.Equals(roleID));

            if (role == null)
            {
                serviceResponse.updateResponse(404, "Không tìm thấy!");

                return serviceResponse;
            }

            var rolePerEx = _dataContext.RolePermissions.Where(e => e.RoleId.Equals(roleID));

            _dataContext.RolePermissions.RemoveRange(rolePerEx);
            await _dataContext.SaveChangesAsync();

            var rolePerDtos = new List<CreateRolePermissionDTO>();
            var rolePerDto = new CreateRolePermissionDTO();

            rolePerDto.RoleId = roleID;

            permissionDTOs.FindAll(e => e.isPermission == true).ForEach(e =>
            {
                rolePerDto.PermissionId = e.Id;
                _dataContext.RolePermissions.Add(_mapper.Map<RolePermission>(rolePerDto));
            });

            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = "Sửa thành công!";

            return serviceResponse;
        }
    }
}
