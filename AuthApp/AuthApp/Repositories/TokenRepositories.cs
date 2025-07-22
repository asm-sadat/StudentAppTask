using AuthApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApp.Repositories
{
    public class TokenRepositories : ITokenRepositories
    {
        private readonly IConfiguration _configuration;
        public TokenRepositories(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateJWTToken(UserModel user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
