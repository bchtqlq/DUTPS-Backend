using System.Text;
using DUTPS.API.Services;
using DUTPS.Databases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static DUTPS.API.Extensions.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

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
      options.OperationFilter<SwaggerParameterFilter>();
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
