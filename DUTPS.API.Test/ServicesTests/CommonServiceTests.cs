using DUTPS.API.Services;
using DUTPS.Databases;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace DUTPS.API.Test.ServicesTests
{
    public class CommonServiceTests : BaseServiceTest
    {

        [Fact]
        public async Task GetFaculties_Test()
        {
            var dbContext = _serviceProvider.GetService<DataContext>();

            var commonService = new CommonService(dbContext);
            // Act
            var result = await commonService.GetFaculties();

            // Assert
            Assert.NotNull(result);
            result.Count.ShouldBeGreaterThan(0);
        }
    }
}