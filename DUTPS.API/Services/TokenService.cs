using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DUTPS.Databases;
using Microsoft.IdentityModel.Tokens;

namespace DUTPS.API.Services
{
    public interface ITokenService
    {
        string CreateToken(string username, int role);
    }
    public class TokenService : ITokenService
    {
        public TokenService(
            )
        {
        }

        public string CreateToken(string username, int role)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, username),
                new Claim(JwtRegisteredClaimNames.Email, $"{username}@dutps.app"),
                new Claim("Role", role.ToString())
            };

            var symmetricKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("My Super SecrectKey OK")
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