using DUTPS.API.Dtos.Commons;
using DUTPS.API.Dtos.Profile;
using DUTPS.API.Dtos.Vehicals;
using DUTPS.Databases;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.API.Services
{
  public interface IUserService
  {
    Task<List<ProfileDto>> GetUsers();
    Task<ProfileDto> GetUserById(int id);

  }
  public class UserService : IUserService
  {
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
      _context = context;
    }

    public async Task<List<ProfileDto>> GetUsers()
    {
      return await _context.Users.Select(x => new ProfileDto
      {
        Id = x.Information.UserId,
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
        Vehicals = x.Vehicals.Select(y => new VehicalDto
        {
          Id = y.Id,
          LicensePlate = y.LicensePlate,
          Description = y.Description
        }).ToList()
      }).ToListAsync();
    }

    public async Task<ProfileDto> GetUserById(int id)
    {
      return await _context.Users.Select(x => new ProfileDto
      {
        Id = x.Information.UserId,
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
        Vehicals = x.Vehicals.Select(y => new VehicalDto
        {
          Id = y.Id,
          LicensePlate = y.LicensePlate,
          Description = y.Description
        }).ToList()
      }).FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}
