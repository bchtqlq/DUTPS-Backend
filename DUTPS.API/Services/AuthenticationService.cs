using System.Security.Cryptography;
using System.Text;
using DUTPS.API.Dtos.Authentication;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using DUTPS.Databases;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.API.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseInfo> Login(UserLoginDto userLoginDto);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AuthenticationService(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<ResponseInfo> Login(UserLoginDto userLoginDto)
        {
            var responeInfo = new ResponseInfo();

            userLoginDto.Username = userLoginDto.Username.ToLower();

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == userLoginDto.Username);

            if (currentUser == null)
            {
                responeInfo.Code = CodeResponse.NOT_FOUND;
                responeInfo.Message = "Invalid username or password";
                return responeInfo;
            }
            
            using var hmac = new HMACSHA512(currentUser.PasswordSalt);

            var passwordBytes = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(userLoginDto.Password)
            );

            for (int i = 0; i < currentUser.PasswordHash.Length; ++i)
            {
                if (currentUser.PasswordHash[i] != passwordBytes[i])
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Invalid username or password";
                    return responeInfo;
                }
            }

            var token = _tokenService.CreateToken(currentUser.Username);
            
            responeInfo.Code = CodeResponse.OK;
            responeInfo.Data.Add("accessToken", token);
            responeInfo.Data.Add("username", userLoginDto.Username);
            return responeInfo;
        }
    }
}