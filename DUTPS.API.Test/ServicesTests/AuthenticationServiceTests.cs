using DUTPS.API.Dtos.Authentication;
using DUTPS.API.Dtos.Profile;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Databases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace DUTPS.API.Test.ServicesTests
{
    public class AuthenticationServiceTests : BaseServiceTest
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _dbContext; 
        private readonly ILogger<AuthenticationService> _logger;
        public AuthenticationServiceTests()
        {
            _dbContext = _serviceProvider.GetService<DataContext>();
            _tokenService = _serviceProvider.GetService<ITokenService>();

            _logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger<AuthenticationService>();
        }

        [Fact]
        public async Task GetProfile_Success()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var username = "admin";

            // Act
            var result = await authService.GetProfile(username);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ChangePassword_Wrong_Username()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var username = "admin1";
            var passwordDto = new ChangePasswordDto()
            {
                OldPassword = "",
                NewPassword = "",
                ConfirmPassword = ""
            };

            // Act
            var result = await authService.ChangePassword(username, passwordDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_FOUND);
            result.Message.ShouldContain("Invalid username");
        }

        [Fact]
        public async Task ChangePassword_Wrong_Password()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var username = "admin";
            var passwordDto = new ChangePasswordDto()
            {
                OldPassword = "Pa$$w0rd1",
                NewPassword = "",
                ConfirmPassword = ""
            };

            // Act
            var result = await authService.ChangePassword(username, passwordDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_FOUND);
            result.Message.ShouldContain("Invalid password");
        }

        [Fact]
        public async Task ChangePassword_Success_Confirm_Password_Not_Match()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var username = "admin";
            var passwordDto = new ChangePasswordDto()
            {
                OldPassword = "Pa$$w0rd",
                NewPassword = "NewPa$$w0rd",
                ConfirmPassword = "OldPa$$w0rd"
            };

            // Act
            var result = await authService.ChangePassword(username, passwordDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_VALIDATE);
        }

        [Fact]
        public async Task UpdateProfile_Success()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var username = "admin";
            var profileDto = new UpdateProfileDto()
            {
                Name = "ADMIN",
                PhoneNumber = "0932046296",
                Class = "19TCLC_DT4",
                FacultyId = "CNTT"
            };

            // Act
            var result = await authService.UpdateProfile(username, profileDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.OK);
            result.Data.ShouldContainKey("username");
        }

        [Fact]
        public async Task UpdateProfile_WrongUsername()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var username = "admin1";
            var profileDto = new UpdateProfileDto()
            {
                Name = "ADMIN"
            };

            // Act
            var result = await authService.UpdateProfile(username, profileDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_FOUND);
        }

        [Fact]
        public async Task Login_Success()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var userLoginDto = new UserLoginDto()
            {
                Username = "admin",
                Password = "Pa$$w0rd",
            };

            // Act
            var result = await authService.Login(userLoginDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.OK);
            result.Data.ShouldContainKey("accessToken");
        }

        [Fact]
        public async Task Login_NotFoundUsername()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var userLoginDto = new UserLoginDto()
            {
                Username = "admin1",
                Password = "pa$$w0rd",
            };

            // Act
            var result = await authService.Login(userLoginDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_FOUND);
            result.Message.ShouldContain("Invalid username or password");
        }

        [Fact]
        public async Task Login_Password_Incorrect()
        {
            //assert   
            var authService = new AuthenticationService(
                _dbContext,
                _logger,
                _tokenService);
            var userLoginDto = new UserLoginDto()
            {
                Username = "admin",
                Password = "pa$$w0rd1",
            };

            // Act
            var result = await authService.Login(userLoginDto);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_FOUND);
            result.Message.ShouldContain("Invalid username or password");
        }
    }
}