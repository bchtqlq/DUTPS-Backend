using DUTPS.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DUTPS.API.Test.ServicesTests
{
    public class BaseServiceTest
    {
        protected readonly ServiceProvider _serviceProvider;
        public BaseServiceTest()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DataContext>(options =>
            options.UseNpgsql
            (
                "User ID=mdentaku;Password=tM2QaxQhnx7MGFci6LbxRdJf51EwBkd8;Server=tiny.db.elephantsql.com;Port=5432;Database=mdentaku;"
            ));

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}