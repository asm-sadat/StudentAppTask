using AuthApp.DataAccessLayer;
using AuthApp.DTOs;
using AuthApp.Models;
using AuthApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _dbContext;
        private readonly ITokenRepositories _tokenRepositories;

        public AuthController(AuthDbContext dbContext, ITokenRepositories tokenrepository)
        {
            _dbContext = dbContext;
            _tokenRepositories = tokenrepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var existingUse = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == registerDto.Email);
            if (existingUse != null)
            {
                return BadRequest("User already exists!");
            }

            var hasher = new PasswordHasher<UserModel>();
            var hashedPassword = hasher.HashPassword(existingUse, registerDto.Password);
            var user = new UserModel
            {
                Email = registerDto.Email,
                Password = hashedPassword,
                Userame = registerDto.Username
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return Ok("Registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null)
            {
                return NotFound("Incorrect Email!");
            }

            var hasher = new PasswordHasher<UserModel>();
            var result = hasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Incorrect Password!");
            }

            var jwtToken = _tokenRepositories.CreateJWTToken(user);
            var response = new LoginResponseDto
            {
                JwtToken = jwtToken,
            };

            return Ok(response);
        }
    }
}
