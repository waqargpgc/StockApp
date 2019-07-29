using App.Authorization;
using DAL.Repositories;
using Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Data.DefaultSettings
{
    public static class Seeddb
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string userPass = null)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (string.IsNullOrWhiteSpace(userPass))
                {
                    userPass = "Pass@123";
                }

                // Super Admin 
                string SuperAdminEmail = "auth@superadmin.com";
                string SuperAdminUserName = "superadmin";
                var superAdmin = await EnsureUser(serviceProvider, userPass, SuperAdminEmail, SuperAdminUserName);
                await CreateRoles(serviceProvider, superAdmin, Constants.SuperAdminRole);
                 
                SeedDB(context, serviceProvider);
            }
        }

        private static async Task<ApplicationUser> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string email, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    SharedKey = Guid.NewGuid().ToString()
                };
                var res = await userManager.CreateAsync(user, testUserPw);
            }

            return user;
        }

        //private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string userId, string roleName)
        //{
        //    IdentityResult IdResult = null;
        //    var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
        //    var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
        //    var user = await userManager.FindByIdAsync(userId);

        //    if (roleManager == null)
        //    {
        //        throw new Exception("roleManager null");
        //    }
            
        //    IdResult = await userManager.AddToRoleAsync(user, roleName);
        //    return IdResult;
        //}

        private static async Task<IdentityResult> CreateRoles(IServiceProvider serviceProvider, ApplicationUser user,  string roleName = null)
        {
            IdentityResult IdResult = null;
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var permissionRepo = serviceProvider.GetService<IUnitOfWork>().Permissions;

            //var superAdmin = "superadmin-" + user.SharedKey.Substring(0, 12);
            var superAdmin = "super-admin";

            var superAdminRole = new ApplicationRole  
            {
                Name = superAdmin,
                CreatedBy = user.Id,
                SharedKey = user.SharedKey+ "_opac",
                IsActive = true,
                DisplayName = "Super Admin"
            };

            var admin = "admin-" + user.SharedKey.Substring(0, 12);
            var adminRole = new ApplicationRole
            {
                Name = admin,
                CreatedBy = user.Id,
                SharedKey = user.SharedKey,
                IsActive = true,
                DisplayName = "Admin"
            };

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(superAdmin))  
            {
                IdResult = await roleManager.CreateAsync(superAdminRole); 
               if(IdResult.Succeeded)
                    IdResult = await roleManager.CreateAsync(adminRole);
            }

            IdResult = await userManager.AddToRoleAsync(user, superAdminRole.Name);
            IdResult = await userManager.AddToRoleAsync(user, adminRole.Name);
            var allPermissions = permissionRepo.GetAllPermissions(); 

            foreach (string claim in allPermissions.Distinct()) 
            {
                //var result = await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Name, claim.Value)); 
                var result = await roleManager.AddClaimAsync(superAdminRole, new Claim(claim, claim));  
            }

            return IdResult;
        }

        public static void SeedDB(AppDbContext context, IServiceProvider serviceProvider)
        {
            var permissionRepo = serviceProvider.GetService<IUnitOfWork>().Permissions;

            if (!context.ApplicationClaims.Any())
            {
                var defPermissions = permissionRepo.GetAllPermissions();
                context.ApplicationClaims.AddRange(defPermissions);
            }

            context.SaveChanges(); 
        }
    }
}
