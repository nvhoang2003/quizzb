using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.RoleServices
{
    public class RoleServices : IRoleServices
    {

        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        public RoleServices(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }
        public RoleServices()
        {
        }

        public async Task<ServiceResponse<RoleResponseDTO>> CreateNewRole(CreateRoleDTO createRoleDTO)
        {
            var serviceResponse = new ServiceResponse<RoleResponseDTO>();
            Role roleSaved = _mapper.Map<Role>(createRoleDTO);

            _dataContext.Roles.Add(roleSaved);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Tạo thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<PageList<RoleDTO>>> GetAllRole(OwnerParameter ownerParameters)
        {
            var serviceResponse = new ServiceResponse<PageList<RoleDTO>>();
            var dbRole= await _dataContext.Roles.ToListAsync();
            var roleDTO = dbRole.Select(u => _mapper.Map<RoleDTO>(u)).ToList();

            serviceResponse.Data = PageList<RoleDTO>.ToPageList(
            roleDTO.AsEnumerable<RoleDTO>(),
            ownerParameters.pageIndex,
            ownerParameters.pageSize);

            return serviceResponse;
        }

        public async Task<ServiceResponse<RoleDTO>> UpdateRole(CreateRoleDTO updateRoleDTO, int id)
        {
            var serviceResponse = new ServiceResponse<RoleDTO>();
            var dbRole = await _dataContext.Roles.FirstOrDefaultAsync(q => q.Id == id);

            if (dbRole == null)
            {
                serviceResponse.updateResponse(404, "Role không tồn tại");
                return serviceResponse;
            }
            if (dbRole.AcceptChange==0)
            {
                serviceResponse.updateResponse(403, "Role này không thể thay đổi");
                return serviceResponse;
            }

            dbRole.Name = updateRoleDTO.Name;
            dbRole.Description = updateRoleDTO.Description;
            
            _dataContext.Roles.Update(dbRole);
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Update thành công");
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoleDTO>> GetRoleByID(int id)
        {
            var serviceResponse = new ServiceResponse<RoleDTO>();
            var dbRole = await _dataContext.Roles.ToListAsync();

            var roleDTO = dbRole.Select(u => _mapper.Map<RoleDTO>(u)).Where(c => c.Id == id).FirstOrDefault();
            if (roleDTO == null)
            {
                serviceResponse.updateResponse(404, "không tồn tại");
                return serviceResponse;
            }

            serviceResponse.Data = roleDTO;
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoleDTO>> DeleteRole(int id)
        {
            var serviceResponse = new ServiceResponse<RoleDTO>();
            var dbRole= await _dataContext.Roles.FirstOrDefaultAsync(q => q.Id == id);

            if (dbRole == null)
            {
                serviceResponse.updateResponse(404, "Role không tồn tại");
                return serviceResponse;
            }
            if (dbRole.AcceptChange==0) {
                serviceResponse.updateResponse(403, "Role này không thể xóa ");
                return serviceResponse;

            }

            dbRole.IsDeleted = 1;
            _dataContext.Roles.Update(dbRole);

            await _dataContext.SaveChangesAsync();
            serviceResponse.updateResponse(200, "Delete thành công");

            return serviceResponse;
        }

    }
}
