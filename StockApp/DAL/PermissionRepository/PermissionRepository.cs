using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Identity;
using StockApp.Data;

namespace DAL.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;
        private readonly List<ApplicationClaim> _permissions;

        public PermissionRepository(AppDbContext context)
        {
            _context = context;
            _permissions =  PermissionManager.AllPermissions.ToList();
        }

        public string[] GetAdministrativePermissionValues()
        {
          return new string[] {
              PermissionManager.ManageUsers,
              PermissionManager.DeleteUsers,
              PermissionManager.UpdateUsers,
              PermissionManager.CreateUsers,
              PermissionManager.ManageRoles,
              PermissionManager.AssignRoles,
              PermissionManager.DeleteRoles,
              PermissionManager.UpdateRoles,
              PermissionManager.CreateRoles,
          };
        }

        public IEnumerable<ApplicationClaim> GetAllPermissions()
        {
            return _permissions;
        }

        public IEnumerable<ApplicationClaim> GetAllPermissionsFromDb()
        {
           // throw new NotImplementedException();
           return _context.ApplicationClaims.ToList();
        }


        public string[] GetAllPermissionValues()
        {
            return _permissions.Select(p => p.Value).ToArray(); 
        }

        public ApplicationClaim GetPermissionByName(string permissionName)
        {
            return _permissions.Where(p => p.Name == permissionName).FirstOrDefault(); 
        }

        public ApplicationClaim GetPermissionByValue(string permissionValue)
        {
            return _permissions.FirstOrDefault(p => p.Value == permissionValue); 
        }  
    }
} 