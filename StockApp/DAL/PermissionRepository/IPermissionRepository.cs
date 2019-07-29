using Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IPermissionRepository
    {
        ApplicationClaim GetPermissionByName(string permissionName);
        ApplicationClaim GetPermissionByValue(string permissionValue);
        string[] GetAllPermissionValues();
        string[] GetAdministrativePermissionValues();
        IEnumerable<ApplicationClaim> GetAllPermissions();
        IEnumerable<ApplicationClaim> GetAllPermissionsFromDb();
    }
}
