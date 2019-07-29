using App.Models.Identity;
using Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StockApp.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signinManager;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signinManager = signinManager;
            _config = config; 
        }


        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized("Invalid email or username."); ; 
            if (!ModelState.IsValid) return response;

            //var existUser = await _userManager.FindByNameAsync(login.UserName);
            //var listRoles = await _userManager.GetRolesAsync(existUser);

            //if (existUser == null)
            //{
            //    return Unauthorized();
            //} 
              
            var user = await Authenticate(login);
            if (user == null) return response;

                var tokenString = await BuildToken(user);
                var requestTime = DateTime.Now;
                response = Ok(
                    new {
                        token = tokenString,
                        roles = user.Roles, 
                        expiresIn = DateTime.Now.AddMinutes(60 * 6),// issue token for 6 hours
                        requestAt = requestTime,
                        token_type = "bearer",
                    });
            
            return response;
        }

        [NonAction]
        private async Task<string> BuildToken(UserModel user)
            {
                var fullName = $"{user.FirstName} {user.LastName}";

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.UserName),
                    new Claim("Key", user.SharedKey),
                    new Claim("Name", fullName)
                };

            claims.AddRange(user.Claims);
            foreach (var roleName in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims.DistinctBy(c => c.Value),
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
         

        [NonAction]
        private async Task<UserModel> Authenticate(LoginModel login)
        {
            UserModel userModel = null;
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null) 
                user = await _userManager.FindByEmailAsync(login.UserName);

            if (user != null)
            {
                var result = await _signinManager.PasswordSignInAsync(user, login.Password, false, false);

                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    userModel = new UserModel
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedDate = DateTime.Now,
                        Designation = user.Designation,
                        Email = user.Email,
                        Id = user.Id,
                        SharedKey = user.SharedKey,
                        Roles = userRoles,
                        Claims = userClaims
                    };
                }
            }

            return userModel;
        }

    }
} 


