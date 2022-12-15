using System.Text;
using DUTPS.API.Services;
using DUTPS.Databases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Sentry;
using (SentrySdk.Init(o =>
	{
		// Tells which project in Sentry to send events to:
		o.Dsn = "https://71898d25b0fd454ab0d6562aba0c73a8@o4504259552477184.ingest.sentry.io/4504259556474881";
		// When configuring for the first time, to see what the SDK is doing:
		o.Debug = true;
		// Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
		// We recommend adjusting this value in production.
		o.TracesSampleRate = 1.0;
		// Enable Global Mode if running in a client app
		o.IsGlobalModeEnabled = true;
	}
	)
)
{

    var builder = WebApplication.CreateBuilder(args);

    ConfigureLogging();

    builder.Host.UseSerilog();

    var services = builder.Services;

    var connectionString = builder.Configuration.GetConnectionString("Default");

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    services
        .AddSwaggerGen(options =>
        {
          options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

          options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
          {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
          });

          options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });
    services.AddDbContext<DataContext>(options =>
        options.UseNpgsql
        (
            connectionString
        )
    );

    services
        .AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = false;
          options.SaveToken = true;
          options.Events = new JwtBearerEvents
          {
            OnMessageReceived = context =>
            {
              var accessToken = context.Request.Query["Token"];
              var path = context.HttpContext.Request.Path;
              if (!String.IsNullOrEmpty(accessToken.ToString()) &&
                      (path.ToString().StartsWith("/hub/")))
              {
                context.Token = accessToken;
              }
              return Task.CompletedTask;
            }
          };
          options.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])
              )
          };
        });



    services.AddTransient<ITokenService, TokenService>();
    services.AddTransient<IAuthenticationService, AuthenticationService>();
    services.AddTransient<ICommonService, CommonService>();
    services.AddTransient<IVehicalService, VehicalService>();
    services.AddTransient<ICheckInService, CheckInService>();
    services.AddTransient<IUserService, UserService>();

    services.AddCors(o =>
                    o.AddPolicy("CorsPolicy", builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()));

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
      var context = scope.ServiceProvider.GetService<DataContext>();
      Seed.SeedUsers(context);
      Seed.SeedFaculties(context);
    }
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    app.UseSentryTracing();

}



void ConfigureLogging()
{
  var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
  var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
      $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
      optional: true)
    .Build();

  Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Debug()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
    .Enrich.WithProperty("Environment", environment)
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
}
ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
  return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
  {
    AutoRegisterTemplate = true,
    IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
  };
}
