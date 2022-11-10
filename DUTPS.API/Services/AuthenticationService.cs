using System.Security.Cryptography;
using System.Text;
using DUTPS.API.Dtos.Authentication;
using DUTPS.API.Dtos.Profile;
using DUTPS.Commons.CodeMaster;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using DUTPS.Databases;
using DUTPS.Databases.Schemas.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DUTPS.API.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseInfo> Login(UserLoginDto userLoginDto);
        Task<ResponseInfo> Register(UserRegisterDto userRegisterDto);
        Task<ProfileDto> GetProfile();
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AuthenticationService(
            DataContext context,
            ITokenService tokenService
        )
        {
            _context = context;
            _tokenService = tokenService;
        }

        public Task<ProfileDto> GetProfile()
        {
            throw new NotImplementedException();
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

        public async Task<ResponseInfo> Register(UserRegisterDto userRegisterDto)
        {
            IDbContextTransaction transaction = null;
            try
            {
                var responeInfo = new ResponseInfo();

                userRegisterDto.Username = userRegisterDto.Username.ToLower();

                if (await _context.Users.AnyAsync(u => u.Username == userRegisterDto.Username))
                {
                    responeInfo.Code = CodeResponse.HAVE_ERROR;
                    responeInfo.Message = "Username is existed";
                    return responeInfo;
                }

                if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
                {
                    responeInfo.Code = CodeResponse.NOT_VALIDATE;
                    responeInfo.Message = "Password does not match";
                    return responeInfo;
                }

                using var hmac = new HMACSHA512();

                var user = new User
                {
                    Username = userRegisterDto.Username,
                    Email = userRegisterDto.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password)),
                    PasswordSalt = hmac.Key,
                    Status = AccountState.Normal.CODE,
                    Role = Role.Customer.CODE
                };

                await _context.Users.AddAsync(user);
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();

                await _context.UserInfos.AddAsync(new UserInfo {UserId = user.Id});
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                responeInfo.Code = CodeResponse.OK;
                responeInfo.Data.Add("accessToken", _tokenService.CreateToken(user.Username));
                responeInfo.Data.Add("username", user.Username);
                return responeInfo;
            }
            catch (Exception)
            {
                await DataContext.RollbackAsync(transaction);
                throw;
            }
        }
    }
}