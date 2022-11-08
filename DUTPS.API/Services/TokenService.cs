using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DUTPS.Databases;
using Microsoft.IdentityModel.Tokens;

namespace DUTPS.API.Services
{
    public interface ITokenService
    {
        string CreateToken(string username);
    }
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public TokenService(
            IConfiguration configuration, 
            DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string CreateToken(string username)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, username),
                new Claim(JwtRegisteredClaimNames.Email, $"{username}@dutps.app")
            };

            var symmetricKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["TokenKey"])
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    symmetricKey, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token  = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}