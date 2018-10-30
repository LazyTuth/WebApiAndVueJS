using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DemoApp.API.Data;
using DemoApp.API.Dtos;
using DemoApp.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DemoApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto)
        {
            userDto.Username = userDto.Username.ToLower();

            var userToCreate = new User
            {
                UserName = userDto.Username,
                FirstName = userDto.Firstname,
                LastName = userDto.Lastname,
                Email = userDto.Email
                //RoleId = 1
            };

            var result = await _userManager.CreateAsync(userToCreate, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userToCreate, "Member");
                return StatusCode(201);
            }
            return BadRequest(result.Errors);
        }

        public string CreateJwtToken(List<Claim> claims, DateTime expirationDate)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = expirationDate
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userDto)
        {
            userDto.Username = userDto.Username.ToLower();
            var user = await _userManager.FindByNameAsync(userDto.Username);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
            if (result.Succeeded)
            {
                // Jwt
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var expirationDate = DateTime.Now.AddHours(5);
                var token = CreateJwtToken(claims, expirationDate);

                return Ok(new
                {
                    userId = user.Id,
                    userFullname = user.FirstName + ' ' + user.LastName,
                    token = token,
                    roles = roles,
                    expiration = expirationDate
                });
            }
            return Unauthorized();
        }
    }
}