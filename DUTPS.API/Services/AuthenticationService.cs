using System.Security.Cryptography;
using System.Text;
using DUTPS.API.Dtos.Authentication;
using DUTPS.API.Dtos.Profile;
using DUTPS.API.Dtos.Vehicals;
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
        Task<ProfileDto> GetProfile(string username);
        Task<ResponseInfo> UpdateProfile(string username, UpdateProfileDto profileDto);
        Task<ResponseInfo> ChangePassword(string username, ChangePasswordDto changePasswordDto);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ITokenService _tokenService;

        public AuthenticationService(
            DataContext context,
            ILogger<AuthenticationService> logger,
            ITokenService tokenService
        )
        {
            _context = context;
            _logger = logger;
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
            responeInfo.Data.Add("roleId", currentUser.Role);
            switch (currentUser.Role)
            {
                case 10:
                    responeInfo.Data.Add("role", Role.Admin.NAME);
                    break;
                case 20:
                    responeInfo.Data.Add("role", Role.Staff.NAME);
                    break;
                case 30:
                    responeInfo.Data.Add("role", Role.Customer.NAME);
                    break;
                default:
                    break;
            }
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
     
        public async Task<ProfileDto> GetProfile(string username)
        {
            username = username.ToLower();
            return await _context.Users.Select(x => new ProfileDto {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                Role = x.Role,
                Name = x.Information.Name,
                Gender = x.Information.Gender,
                Birthday = x.Information.Birthday,
                PhoneNumber = x.Information.PhoneNumber,
                Class = x.Information.Class,
                FacultyId = x.Information.FacultyId,
                FalcultyName = x.Information.Faculty.Name,
                Vehicals = x.Vehicals.Select(y => new VehicalDto {
                    Id = y.Id,
                    LicensePlate = y.LicensePlate,
                    Description = y.Description}).ToList()
            }).FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<ResponseInfo> UpdateProfile(string username, UpdateProfileDto profileDto)
        {
            IDbContextTransaction transaction = null;
            try
            {
                var responeInfo = new ResponseInfo();
                username = username.ToLower();
                var currentUser = await _context.Users.Include(x => x.Information).FirstOrDefaultAsync(x => x.Username == username);

                if (currentUser == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Not found user";
                    return responeInfo;
                }

                _logger.LogInformation("Ok");

                currentUser.Information.Name = profileDto.Name;
                currentUser.Information.Birthday = profileDto.Birthday;
                currentUser.Information.Gender = profileDto.Gender;
                currentUser.Information.FacultyId = profileDto.FacultyId;
                currentUser.Information.Class = profileDto.Class;
                currentUser.Information.PhoneNumber = profileDto.PhoneNumber;

                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Data.Add("username", currentUser.Username);
                return responeInfo;
            }
            catch (Exception)
            {
                await DataContext.RollbackAsync(transaction);
                throw;
            }
        }

        public async Task<ResponseInfo> ChangePassword(string username, ChangePasswordDto changePasswordDto)
        {
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation("Begin Change Password");
                var responeInfo = new ResponseInfo();

                username = username.ToLower();

                var currentUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (currentUser == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Invalid username";
                    _logger.LogError("Invalid username");
                    return responeInfo;
                }
                
                using var hmac = new HMACSHA512(currentUser.PasswordSalt);

                var passwordBytes = hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(changePasswordDto.OldPassword)
                );

                for (int i = 0; i < currentUser.PasswordHash.Length; ++i)
                {
                    if (currentUser.PasswordHash[i] != passwordBytes[i])
                    {
                        responeInfo.Code = CodeResponse.NOT_FOUND;
                        responeInfo.Message = "Invalid password";
                        _logger.LogError("Invalid password");
                        return responeInfo;
                    }
                }

                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                {
                    responeInfo.Code = CodeResponse.NOT_VALIDATE;
                    responeInfo.Message = "Password does not match";
                    _logger.LogError("Password does not match");
                    return responeInfo;
                }

                using var newHmac = new HMACSHA512();

                currentUser.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(changePasswordDto.NewPassword));
                currentUser.PasswordSalt = newHmac.Key;

                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Data.Add("username", currentUser.Username);
                _logger.LogInformation("Change password success");
                return responeInfo;
            }
            catch (Exception e)
            {
                await DataContext.RollbackAsync(transaction);
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }
    }
}