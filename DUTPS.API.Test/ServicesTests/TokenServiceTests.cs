using DUTPS.API.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DUTPS.API.Test.ServicesTests
{
    public class TokenServiceTests
    {
        [Fact]
        public void Create_TokenTest()
        {
            // Arrange
            var config = new Mock<IConfiguration>();
            config.Setup(cf => cf["TokenKey"]);
            string username = "admin";
            var tokenService = new TokenService(config.Object);
            // Act
            var result = tokenService.CreateToken(username, 10);

            // Assert
            Assert.NotNull(result);
        }
    }
}