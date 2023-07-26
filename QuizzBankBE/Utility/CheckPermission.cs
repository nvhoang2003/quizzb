using QuizzBankBE.DataAccessLayer.Data;

namespace QuizzBankBE.Utility
{
    public class CheckPermission
    {
        public static bool check(int userIdLogin, string permissionName)
        {
            DataContext _dataContext = new DataContext();

            return (from u in _dataContext.Users
                    join r in _dataContext.Roles on u.RoleId equals r.Id
                    join rp in _dataContext.RolePermissions on r.Id equals rp.RoleId
                    join p in _dataContext.Permissions on rp.PermissionId equals p.Id
                    where u.Id == userIdLogin
                    where p.Name == permissionName
                    select p).FirstOrDefault() != null;
        }
    }
}
