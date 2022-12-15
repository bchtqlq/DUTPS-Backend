using DUTPS.API.Dtos.Vehicals;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Databases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace DUTPS.API.Test.ServicesTests
{
    public class VehicalServiceTests : BaseServiceTest
    {
        private readonly DataContext _dbContext; 
        private readonly ILogger<VehicalService> _logger;
        public VehicalServiceTests()
        {
            _dbContext = _serviceProvider.GetService<DataContext>();

            _logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger<VehicalService>();
        }

        [Fact]
        public async Task AddVehical_Success()
        {
            //assert   
            var authService = new VehicalService(
                _dbContext,
                _logger
                );
            var username = "admin";
            var vehical = new VehicalCreateUpdateDto()
            {
                LicensePlate = "72E1-24191",
                Description = "Sirius"
            };

            // Act
            var result = await authService.AddVehical(username, vehical);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.OK);
            result.Data.ShouldContainKey("vehical");
        }

        [Fact]
        public async Task AddVehical_Fail()
        {
            //assert   
            var authService = new VehicalService(
                _dbContext,
                _logger
                );
            var username = "admin1";
            var vehical = new VehicalCreateUpdateDto()
            {
                LicensePlate = "72E1-24191",
                Description = "Sirius"
            };

            // Act
            var result = await authService.AddVehical(username, vehical);

            // Assert
            Assert.NotNull(result);
            result.Code.ShouldBe(CodeResponse.NOT_FOUND);
        }
    }
}