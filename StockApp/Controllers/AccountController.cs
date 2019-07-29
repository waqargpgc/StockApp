using App.Authorization;
using App.Models.Identity;
using AutoMapper;
using DAL.Repositories;
using Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockApp.Data;
using StockApp.Data.Enitities;
using StockApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Controllers
{
    [Authorize]
    [Route("api/Account")]
    public class AccountController : ControllerBase
    {
        public static string supAdminRole = "super-admin";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;
        private readonly string _key;
        private readonly IUnitOfWork _uow;

        #region constructor
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IUnitOfWork uow,
            IHttpContextAccessor accessor,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<CustomerController>();
            _mapper = mapper;
            _uow = uow;
            _key = accessor?.HttpContext?.User?.FindFirst("Key")?.Value;
        }
        #endregion

        #region Create new user 
        // api/account/createuser
        [Authorize(Policy = "create-users")]
        [HttpPost, Route("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateModel user)
        {
            var response = BadRequest(new Result { Success = false });
            var loggedInUser = await GetUser();

            //var supAdminRole = "superadmin-" + loggedInUser.SharedKey.Substring(0, 12).Trim().ToLowerInvariant();
            //var supAdminRole = "super-admin";
            var ownerRole = "owner-" + _key.Substring(0, 12).Trim().ToLowerInvariant();
             
            // if state is not valid then retrurn 
            if (!ModelState.IsValid)
            { 
                return BadRequest(new {
                    Success = false,
                    Errors = new SerializableError(ModelState)
                });
            };

            // super admin can only create owner
            if ((User.IsInRole(supAdminRole)) && !user.IsOwner) return response;

            //// allow only owner or superadmin. 
            //if ((!User.IsInRole(supAdminRole)) && (!User.IsInRole(ownerRole))) return response;

            // username is optional. 
            if (string.IsNullOrWhiteSpace(user.UserName)) user.UserName = user.Email;

            #region create user
            var newUser = _mapper.Map<ApplicationUser>(user);

            // if new user is owner created by superadmin then create new 'Key', otherwise share loggedin user key 
            if (user.IsOwner && User.IsInRole(supAdminRole))
                newUser.SharedKey = Guid.NewGuid().ToString();
            else
                newUser.SharedKey = _key;

            newUser.CreatedBy = loggedInUser.Id;
            newUser.CreatedDate = DateTime.Now;
            newUser.IsActive = true;

            // create new user, if not succeeded then return
            var idResult = await _userManager.CreateAsync(newUser, user.Password);
            if (!idResult.Succeeded)
            {
                response = BadRequest(new Result()
                {
                    Success = false,
                    Errors = idResult.Errors.Select(e => e.Description).ToArray(),
                    Data = null
                });

                return response;
            }

            newUser = await _userManager.FindByNameAsync(newUser.UserName);
            #endregion

            #region Assign roles to new user
            if (!User.IsInRole(supAdminRole))
            {
                try
                {
                    var roles = user.Roles.Where(x => x.Trim().ToLowerInvariant() != ownerRole).Distinct();
                    bool allRolesExist = true;

                    foreach (var roleName in roles)
                    {
                        // ensure all roles exist. if not then create
                        if (!(await _roleManager.RoleExistsAsync(roleName)))
                        {
                            allRolesExist = false;
                            //var role = new ApplicationRole
                            //{
                            //    Name = roleName,
                            //    DisplayName = roleName,
                            //    CreatedBy = loggedInUser.Id,
                            //    SharedKey = _key,
                            //    IsActive = true,
                            //    CreatedDate = DateTime.Now
                            //};

                            //await _roleManager.CreateAsync(role);
                        }

                    }

                    // Assign roles to new user
                    if (allRolesExist)
                        idResult = await _userManager.AddToRolesAsync(newUser, roles);
                    else
                        throw new Exception("found invalid roles");

                }
                catch (Exception ex)
                {
                    await DeleteUserAsync(newUser);
                    response = BadRequest(new Result()
                    {
                        Success = false,
                        Errors = idResult.Errors.Select(e => e.Description).ToArray(),
                        Data = null
                    });

                    return response;
                }

                if (!idResult.Succeeded)
                {
                    await DeleteUserAsync(newUser);
                    response = BadRequest(new Result()
                    {
                        Success = false,
                        Errors = idResult.Errors.Select(e => e.Description).ToArray(),
                        Data = null
                    });

                    return response;
                }
            }
            #endregion

            #region create default setting
            try
            {
                // allow only super admin to create owner
                if (User.IsInRole(supAdminRole))
                {
                    // if owner, then create default setting
                    if (user.IsOwner)
                    {
                        var defaultLocation = new InventoryLocation
                        {
                            LocationName = "Default Location"
                        };

                        _uow.Inventories.AddLocation(defaultLocation, newUser);
                        var isSuccess = _uow.Commit();

                        if (isSuccess) isSuccess = await CreateDefaultPermissions(newUser);
                        if (!isSuccess) throw new Exception("something went wrong");
                    }
                }
            }
            catch (Exception ex)
            {
                await DeleteUserAsync(newUser);
                response = BadRequest(new Result()
                {
                    Success = false,
                    Errors = idResult.Errors.Select(e => e.Description).ToArray(),
                    Data = null
                });

                return response;
            }

            #endregion


            var userRoles = new List<string>();
            var roleNames = await _userManager.GetRolesAsync(newUser);
            foreach (var name in roleNames.Distinct())
            {
                var role = _roleManager.Roles.Where(x => x.Name == name).FirstOrDefault();

                if (role != null) userRoles.Add(role.DisplayName);
            }

            return Ok(new Result()
            {
                Success = true,
                Data = new
                {
                    User = newUser,
                    Roles = userRoles
                }
            });
        }
        #endregion

        #region Delete User
        // api/account/deleteuser/id
        [HttpDelete, Route("deleteuser/{id}")]
        [Authorize(Policy = "delete-users")]
        public async Task<IActionResult> DeleteUserByIdAsync(string id)
        {
            var result = new Result();
            try
            {
                // __::Todo - add contraints, who can delete...
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound(result);

                var idResult = await DeleteUserAsync(user);
                if (!idResult.Succeeded)
                {
                    result.Errors = idResult.Errors.Select(e => e.Description).ToArray();
                    return BadRequest(result);
                }

                result.Success = idResult.Succeeded;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return BadRequest(result);
            }
        }
        #endregion

        #region Create Role
        //api/account/createrole
        [HttpPost, Route("createrole")]
        [Authorize(Policy = "create-roles")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateModel model)
        {
            var result = new Result();
            var role = new ApplicationRole();
            var loggedInUser = await GetUser();
            IdentityResult IdResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new {
                        Errors = new SerializableError(ModelState),
                        Success = false
                    });
                }

                var roleToCreate = model.RoleName + "-" + _key.Substring(0, 12);
                if (model.Claims == null) model.Claims = new string[] { };
                var invalidClaims = model.Claims.Where(c => _uow.Permissions.GetAllPermissions().Where(x => x.Value.ToLowerInvariant() == c.ToLowerInvariant()) == null).ToArray();

                if (invalidClaims.Any())
                {
                    result = new Result()
                    {
                        Success = false,
                        Errors = new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) },
                        Data = null
                    };

                    return BadRequest(result);
                }

                if (!(await _roleManager.RoleExistsAsync(roleToCreate)))
                {
                    role = new ApplicationRole
                    {
                        Name = roleToCreate.Trim().ToLowerInvariant(),
                        DisplayName = model.RoleName,
                        CreatedBy = loggedInUser.Id,
                        SharedKey = _key,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                    };

                    IdResult = await _roleManager.CreateAsync(role);
                    if (!IdResult.Succeeded)
                    {
                        result = new Result()
                        {
                            Success = false,
                            Errors = IdResult.Errors.Select(e => e.Description).ToArray(),
                            Data = null
                        };

                        return BadRequest(result);
                    }

                    role = await _roleManager.FindByNameAsync(role.Name);
                    foreach (string claim in model.Claims.Distinct())
                    {
                        IdResult = await this._roleManager.AddClaimAsync(role, new Claim(claim, claim));

                        if (!IdResult.Succeeded)
                        {
                            await DeleteRoleAsync(role);

                            result = new Result()
                            {
                                Success = false,
                                Errors = IdResult.Errors.Select(e => e.Description).ToArray(),
                            };

                            return BadRequest(result);
                        }
                    }
                }
                else
                {
                    result.AddError("Role already exists");

                    return BadRequest(result);
                }


                result = new Result
                {
                    Success = true,
                    Data = new
                    {
                        role.Id,
                        Role = model.RoleName,
                        model.Claims
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                await DeleteRoleAsync(role);

                result = new Result()
                {
                    Success = false,
                    Errors = IdResult.Errors.Select(e => e.Description).ToArray(),
                };

                return BadRequest(result);
            }

            return BadRequest(result);
        }
        #endregion

        #region Update Role 
        //api/account/updaterole
        [HttpPut, Route("updaterole")]
        [Authorize(Policy = "update-roles")]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] RoleUpdateModel model)
        {
            var result = new Result();
            IdentityResult IdResult = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Errors = new SerializableError(ModelState),
                        Success = false
                    });
                }

                if (model.Claims != null)
                {
                    string[] invalidClaims = model.Claims.Where(c => _uow.Permissions.GetAllPermissions().Where(x => x.Value == c) == null).ToArray();
                    if (invalidClaims.Any())
                    {
                        result = new Result()
                        {
                            Success = false,
                            Errors = new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) },
                            Data = null
                        };

                        return BadRequest(result);
                    }
                }

                var role = _roleManager.Roles.FirstOrDefault(r => r.Id == model.Id);
                if (role == null) return NotFound(result);

                role.DisplayName = model.DisplayName ?? role.DisplayName;
                role.IsActive = model.IsActive;
                IdResult = await _roleManager.UpdateAsync(role);

                if (!IdResult.Succeeded)
                {
                    result = new Result()
                    {
                        Success = false,
                        Errors = IdResult.Errors.Select(e => e.Description).ToArray()
                    };

                    return BadRequest(result);
                }

                if (model.Claims != null)
                {
                    var roleClaims = (await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == CustomClaimTypes.Permission);
                    var roleClaimValues = roleClaims.Select(c => c.Value).ToArray();

                    var claimsToRemove = roleClaimValues.Except(model.Claims).ToArray();
                    var claimsToAdd = model.Claims.Except(roleClaimValues).Distinct().ToArray();

                    if (claimsToRemove.Any())
                    {
                        foreach (string claim in claimsToRemove)
                        {
                            IdResult = await _roleManager.RemoveClaimAsync(role, roleClaims.Where(c => c.Value == claim).FirstOrDefault());
                            if (!IdResult.Succeeded)
                            {
                                result = new Result()
                                {
                                    Success = false,
                                    Errors = IdResult.Errors.Select(e => e.Description).ToArray()
                                };

                                return BadRequest(result);
                            }
                        }
                    }

                    if (claimsToAdd.Any())
                    {
                        foreach (string claim in claimsToAdd)
                        {
                            IdResult = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, _uow.Permissions.GetPermissionByValue(claim)));
                            if (!IdResult.Succeeded)
                            {
                                result = new Result()
                                {
                                    Success = false,
                                    Errors = IdResult.Errors.Select(e => e.Description).ToArray()
                                };

                                return BadRequest(result);
                            }
                        }
                    }
                }

                result.Success = true;
                result.Data = new
                {
                    Role = role,
                    model.Claims
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return BadRequest(result);
            }
        }
        #endregion

        #region Delete Role
        //api/account/deleterole/[id/name]
        [HttpDelete("deleterole/{role}")] 
        [Authorize(Policy = "delete-roles")]
        public async Task<IActionResult> DeleteRoleAsync(string role)
        {
            var result = new Result();
            try
            {
                // _roleManager.FindByIdAsync(id);
                var roleDb = await _roleManager.FindByNameAsync(role);
                if (roleDb == null) roleDb = await _roleManager.FindByIdAsync(role);
                if (roleDb == null) return NotFound();

                var idResult = await DeleteRoleAsync(roleDb);
                if (!idResult.Succeeded) {
                    result.Success = false;
                    result.Errors = idResult.Errors.Select(e => e.Description).ToArray();

                    return BadRequest(result);
                };

                result.Data = role;
                result.Success = idResult.Succeeded;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return BadRequest(result);
            }
        }
        #endregion

        #region Get User GetUsers
        // api/account/getusers
        // __::Todo- filter, sort, search
        [HttpGet, Route("getusers")]
        [Authorize(Policy = "view-users")]
        public async Task<IActionResult> GetUsersWithRolesAsync(UserResourceParameters parameters)
        {
            var result = new Result();
            var loggedInUser = await GetUser();
            try
            {
                var userQry = _userManager.Users.Where(u => u.IsActive && !u.IsDeleted);
                if (!User.IsInRole(supAdminRole)) userQry = userQry.Where(u => u.SharedKey == _key);

                var queryResult = _mapper.Map<IEnumerable<UserListModel>>(userQry).AsQueryable();
                var data = queryResult.GetPaged(parameters.PageNo, parameters.PageSize);

                result.Success = true;
                result.Data = data;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return BadRequest(result);
        }

        // api/account/getusers/id
        [HttpGet("getusers/{searchParam}")]
        [Authorize(Policy = "view-users")]
        public async Task<IActionResult> GetUserInfoAsync(string searchParam)
        {
            var result = new Result { Data = searchParam };
            try
            {
                var user = await _userManager.FindByIdAsync(searchParam);
                if (user == null) user = await _userManager.FindByNameAsync(searchParam);
                if (user == null) user = await _userManager.FindByEmailAsync(searchParam);
                if (user == null) return NotFound(result);

                var userRoles = await GetUserRolesAsync(user);
                var userPermissions = await GetUserPermissionsAsync(user);

                var userInfo = _mapper.Map<UserInfoModel>(user);
                userInfo.Roles = userRoles;
                userInfo.Claims = userPermissions;

                result = new Result
                {
                    Success = true,
                    Data = userInfo,
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return BadRequest(result);
        }

        #endregion

        // api/account/getusers/id
        [HttpPut("updateuser")]
        [Authorize(Policy = "update-users")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateModel updateModel)
        {
            var result = new Result();
            try
            {
                var user = await _userManager.FindByIdAsync(updateModel?.Id);
                if (user == null) return NotFound(result);
                if (user.SharedKey != _key) return Unauthorized();

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Errors = new SerializableError(ModelState),
                        Success = false
                    });
                }

                user.UserName = updateModel.UserName ?? updateModel.Email; // if null default to email
                user.Email = updateModel.Email ?? updateModel.Email;
                user.FirstName = updateModel.FirstName;
                user.LastName = updateModel.LastName;
                user.Designation = updateModel.Designation;
                user.PhoneNumber = updateModel.PhoneNumber;
                user.IsActive = updateModel.IsActive;

                result = await UpdateUserAsync(user, updateModel?.Roles);
                if (!result.Success) return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return BadRequest(result);
        }


        public async Task<Result> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return new Result() { Success = false, Errors = result.Errors.Select(e => e.Description).ToArray() };

            return new Result() { Success = true }; 
        }

        public async Task<Result> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return new Result() { Success = false, Errors = result.Errors.Select(e => e.Description).ToArray() }; // Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            return new Result() { Success = true }; //Tuple.Create(true, new string[] { });
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (!_userManager.SupportsUserLockout) await _userManager.AccessFailedAsync(user);

                return false;
            }

            return true;
        }

        #region Get Roles

        // api/account/getroles/[name/id]
        [HttpGet, Route("getroles/{role}")]
        [Authorize(Policy = "view-roles")]
        public async Task<IActionResult> GetRolesync(string role)
        {
            var result = new Result();

            var roleNDb = await _roleManager.FindByNameAsync(role);
            if(roleNDb == null) roleNDb = await _roleManager.FindByIdAsync(role);

            if (roleNDb == null)
            {
                result.AddError("Role not found.");
                return NotFound(result);
            }

            var claims = await GetRolePermissionsAsync(roleNDb);
            var users = await GetRoleUsersAsync(roleNDb.Name);

            result.Success = true;
            result.Data = new
            {
                roleNDb.Name,
                roleNDb.DisplayName,
                roleNDb.Id,
                Claims = claims,
                Users = users
            };

            return Ok(result);
        }

        // api/account/getroles 
        [HttpGet, Route("getroles")]
        [Authorize(Policy = "view-roles")]
        public async Task<IActionResult> GetRoles(RoleResourceParameters parameters)
        {
            var result = new Result();

            try
            {
                var roleQry = _roleManager.Roles.Where(x => x.IsActive && !x.IsDeleted);
                if (!User.IsInRole(supAdminRole)) roleQry = roleQry.Where(u => u.SharedKey == _key);

                var queryResult = _mapper.Map<IEnumerable<RoleListModel>>(roleQry).AsQueryable();
                // __::todo- apply filter, sort & serach query
                var data = queryResult.GetPaged(parameters.PageNo, parameters.PageSize);

                //var ownerRole = _roleManager.Roles.FirstOrDefault(x => x.SharedKey == user.SharedKey+"_opac" && x.IsActive && !x.IsDeleted); 

                result.Success = true;
                result.Data = data;

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return BadRequest(result);
        }

        // __::Todo get roles details, claims, users etc..
        //public async Task<ApplicationRole> GetRoleLoadRelatedAsync(string roleName)
        //{
        //    var role = await _context.Roles
        //        .Include(r => r.Claims)
        //        .Include(r => r.Users)
        //        .Where(r => r.Name == roleName)
        //        .FirstOrDefaultAsync();

        //    return role;
        //}

        #endregion

        #region  private methods

        #region  Delete User
        private async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            var idResult = new IdentityResult();

            try
            {
                if (user == null) return idResult;
                idResult = await _userManager.DeleteAsync(user);
                return idResult;
            }
            catch
            {
                //__::Todo log detail
            }

            return idResult;
        }
        #endregion

        #region Default Permissions 
        private async Task<bool> CreateDefaultPermissions(ApplicationUser owner)
        {
            if (owner == null) return false;

            IdentityResult IdResult = null;
            try
            {
                var loggedInUser = await GetUser();
                var defOwnerRole = "owner-" + owner.SharedKey.Substring(0, 12);
                var defAdminRole = "admin-" + owner.SharedKey.Substring(0, 12);

                var ownerRole = new ApplicationRole
                {
                    Name = defOwnerRole,
                    DisplayName = "Owner",
                    SharedKey = owner.SharedKey + "_opac",
                    CreatedBy = loggedInUser.Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                };

                var adminRole = new ApplicationRole
                {
                    Name = defAdminRole,
                    DisplayName = "Admin",
                    SharedKey = owner.SharedKey,
                    CreatedBy = loggedInUser.Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                };

                IdResult = await _roleManager.CreateAsync(ownerRole);

                if (IdResult.Succeeded) IdResult = await _roleManager.CreateAsync(adminRole);
                if (IdResult.Succeeded) IdResult = await _userManager.AddToRoleAsync(owner, defAdminRole);
                if (IdResult.Succeeded) IdResult = await _userManager.AddToRoleAsync(owner, defOwnerRole);

                if (IdResult.Succeeded)
                {
                    var defOwnerClaims = _uow.Permissions.GetAllPermissions().Distinct();

                    var role = await _roleManager.FindByNameAsync(defOwnerRole);
                    foreach (var claim in defOwnerClaims)
                    {
                        //var result = await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Name, claim.Value)); 
                        var result = await _roleManager.AddClaimAsync(role, new Claim(claim.Value, claim.Value)); // (type, value)

                        if (!result.Succeeded) return false;
                    }

                }

            }
            catch (Exception)
            {
            }

            return IdResult.Succeeded;
        }
        #endregion

        #region Delete Role 
        private async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) return result;

            return result;
        }
        #endregion

        #region Get User Roles 
        private async Task<IEnumerable<RoleListModel>> GetUserRolesAsync(ApplicationUser user)
        {
            var userRoles = new List<RoleListModel>();
            if (user == null) return userRoles;

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles
                .Where(r => roleNames.Contains(r.Name) && r.IsActive && !r.IsDeleted && r.SharedKey == user.SharedKey);

            userRoles = _mapper.Map<IEnumerable<RoleListModel>>(roles).ToList();
            return userRoles;
        }
        #endregion

        #region User Get User Permissions
        private async Task<IEnumerable<ApplicationClaim>> GetUserPermissionsAsync(ApplicationUser user)
        {
            var userClaims = new List<ApplicationClaim>();
            if (user == null) return userClaims;

            var roleNames = await _userManager.GetRolesAsync(user);
            IEnumerable<Claim> userAssignedClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>();
            claims.AddRange(userAssignedClaims);

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            var permissions = _uow.Permissions.GetAllPermissions();
            var userOwnedClaims = claims.DistinctBy(c => c.Value).Select(c => c.Value);

            userClaims = permissions.Where(p => userOwnedClaims.Contains(p.Value)).ToList();
            return userClaims;
        }
        #endregion

        #region Get Role Permissions
        private async Task<IEnumerable<ApplicationClaim>> GetRolePermissionsAsync(ApplicationRole role)
        {
            var roleClaimsList = new List<ApplicationClaim>();
            if (role == null) return roleClaimsList;

            var claims = await _roleManager.GetClaimsAsync(role); 
            if (!claims.Any()) return roleClaimsList;

            var permissions = _uow.Permissions.GetAllPermissions();
            var roleAssingedClaims = claims.DistinctBy(c => c.Value).Select(c => c.Value);

            roleClaimsList = permissions.Where(p => roleAssingedClaims.Contains(p.Value)).ToList();
            return roleClaimsList;
        }
        #endregion

        #region Get Users in Role
        private async Task<IEnumerable<UserListModel>> GetRoleUsersAsync(string role)
        {
            var roleUsers = new List<UserListModel>();
            if (role == null) return roleUsers;

            var userList = await _userManager.GetUsersInRoleAsync(role);
            roleUsers =  _mapper.Map<IEnumerable<UserListModel>>(userList).ToList();
              
            return roleUsers;
        }
        #endregion

        #region Update User
        public async Task<Result> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            // __:: todo- fix update isseus, who 
            var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new Result()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => e.Description).ToArray()
                    };
                };

            if (roles.Any())
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = userRoles.Except(roles).ToArray();
                var rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Any())
                {
                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!result.Succeeded)
                    {
                        return new Result()
                        {
                            Success = false,
                            Errors = result.Errors.Select(e => e.Description).ToArray()
                        };
                    }
                }

                var rolesInDb = _roleManager.Roles.Select(x => x.Name).ToList();
                rolesToAdd = rolesToAdd.Where(r => rolesInDb.Contains(r)).ToArray(); // exclude roles if not exist. 

                if (rolesToAdd.Any())
                {
                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!result.Succeeded)
                    {
                        return new Result()
                        {
                            Success = false,
                            Errors = result.Errors.Select(e => e.Description).ToArray()
                        };
                    }
                }
            }

            return new Result() { Success = true };
        }
        #endregion

        private IEnumerable<string> GetModelErrors()
        {
            var errors = new List<string>();
            foreach (var modelStateVal in ModelState.Values)
            {
                foreach (var error in modelStateVal.Errors)
                {
                    errors.Add(error.ErrorMessage);
                    var exception = error.Exception;
                }
            }

            return errors;
        }
        #endregion

        [NonAction]
        private async Task<ApplicationUser> GetUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
