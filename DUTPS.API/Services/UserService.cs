using System.Security.Cryptography;
using System.Text;
using DUTPS.API.Dtos.Commons;
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
  public interface IUserService
  {
    Task<List<ProfileDto>> GetUsers();
    Task<ProfileDto> GetUserById(int id);
    Task<ResponseInfo> CreateUser(ProfileDto profile);
    Task<ResponseInfo> UpdateUser(int id, ProfileDto profile);
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

    public async Task<ResponseInfo> CreateUser(ProfileDto profile)
    {
      IDbContextTransaction transaction = null;
      try
      {
        var responeInfo = new ResponseInfo();

        profile.Username = profile.Username.ToLower();

        if (await _context.Users.AnyAsync(u => u.Username == profile.Username))
        {
          responeInfo.Code = CodeResponse.HAVE_ERROR;
          responeInfo.Message = "Username is existed";
          return responeInfo;
        }

        using var hmac = new HMACSHA512();

        var user = new User
        {
          Username = profile.Username,
          Email = profile.Email,
          PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(profile.Password)),
          PasswordSalt = hmac.Key,
          Status = AccountState.Normal.CODE,
          Role = Role.Customer.CODE
        };

        await _context.Users.AddAsync(user);
        transaction = await _context.Database.BeginTransactionAsync();
        await _context.SaveChangesAsync();

        await _context.UserInfos.AddAsync(new UserInfo
        {
          UserId = user.Id,
          Name = profile.Name,
          Gender = profile.Gender,
          Birthday = profile.Birthday,
          PhoneNumber = profile.PhoneNumber,
          Class = profile.Class,
          FacultyId = profile.FacultyId,

        });
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        responeInfo.Code = CodeResponse.OK;
        return responeInfo;
      }
      catch (Exception)
      {
        await DataContext.RollbackAsync(transaction);
        throw;
      }
    }

    public async Task<ResponseInfo> UpdateUser(int id, ProfileDto profile)
    {
      IDbContextTransaction transaction = null;
      try
      {
        var responeInfo = new ResponseInfo();

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        var userInfo = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == id);
        if (user == null || userInfo == null)
        {
          responeInfo.Code = CodeResponse.NOT_FOUND;
          responeInfo.Message = "Not found User";
          return responeInfo;
        }

        user.Username = profile.Username;
        user.Email = profile.Email;
        user.Status = profile.Status;
        user.Role = profile.Role;

        userInfo.UserId = user.Id;
        userInfo.Name = profile.Name;
        userInfo.Gender = profile.Gender;
        userInfo.Birthday = profile.Birthday;
        userInfo.PhoneNumber = profile.PhoneNumber;
        userInfo.Class = profile.Class;
        userInfo.FacultyId = profile.FacultyId;

        transaction = await _context.Database.BeginTransactionAsync();
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        responeInfo.Message = "Update User Success";
        responeInfo.Data.Add("User", user.Id);
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
