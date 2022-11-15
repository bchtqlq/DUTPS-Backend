using DUTPS.API.Dtos.Vehicals;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using DUTPS.Databases;
using DUTPS.Databases.Schemas.Vehicals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DUTPS.API.Services
{
    public interface IVehicalService
    {
        Task<List<VehicalDto>> GetVehicalsByUsername(string username);
        Task<ResponseInfo> AddVehical(string username, VehicalCreateUpdateDto input);
        Task<ResponseInfo> UpdateVehical(long vehicalId, VehicalCreateUpdateDto input);
        Task<ResponseInfo> DeleteVehical(long vehicalId);
        Task<VehicalDto> GetVehicalByVehicalId(long vehicalId);
    }
    public class VehicalService : IVehicalService
    {
        private readonly DataContext _context;
        private readonly ILogger<VehicalService> _logger;

        public VehicalService(
            DataContext context,
            ILogger<VehicalService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseInfo> AddVehical(string username, VehicalCreateUpdateDto input)
        {
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation("Begin create vehical");
                var responeInfo = new ResponseInfo();
                username = username.ToLower();
                var currentUser = await _context.Users.Include(x => x.Information).FirstOrDefaultAsync(x => x.Username == username);

                if (currentUser == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Not found user";
                    return responeInfo;
                }

                var vehical = new Vehical();

                vehical.LicensePlate = input.LicensePlate;
                vehical.Description = input.Description;
                vehical.CustomerId = currentUser.Id;

                await _context.Vehicals.AddAsync(vehical);
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Message = "Create Vehical Success";
                responeInfo.Data.Add("vehical", vehical.Id);
                _logger.LogInformation("Create vehical success");
                return responeInfo;
            }
            catch (Exception e)
            {
                await DataContext.RollbackAsync(transaction);
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }

        public async Task<ResponseInfo> DeleteVehical(long vehicalId)
        {
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation("Begin Delete vehical");
                var responeInfo = new ResponseInfo();

                var vehical = await _context.Vehicals.FirstOrDefaultAsync(x => x.Id == vehicalId);

                if (vehical == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Not found vehical";
                    return responeInfo;
                }

                _context.Vehicals.Remove(vehical);

                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Message = "Delete Vehical Success";
                responeInfo.Data.Add("vehical", vehical.Id);
                _logger.LogInformation("Delete vehical success");
                return responeInfo;
            }
            catch (Exception e)
            {
                await DataContext.RollbackAsync(transaction);
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }

        public async Task<VehicalDto> GetVehicalByVehicalId(long vehicalId)
        {
            var vehical = await _context.Vehicals
                .Select(x => new VehicalDto {
                    Id = x.Id,
                    LicensePlate = x.LicensePlate,
                    Description = x.Description,})
                .FirstOrDefaultAsync(x => x.Id == vehicalId);

            if (vehical == null) return new VehicalDto();
            
            return vehical;
        }

        public async Task<List<VehicalDto>> GetVehicalsByUsername(string username)
        {
            username = username.ToLower();
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (currentUser == null)
            {
                return new List<VehicalDto>();
            }

            return await _context.Vehicals
                .Where(x => x.CustomerId == currentUser.Id)
                .Select(x => new VehicalDto {
                    Id = x.Id,
                    LicensePlate = x.LicensePlate,
                    Description = x.Description,})
                .ToListAsync();
        }

        public async Task<ResponseInfo> UpdateVehical(long vehicalId, VehicalCreateUpdateDto input)
        {
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation("Begin Update vehical");
                var responeInfo = new ResponseInfo();

                var vehical = await _context.Vehicals.FirstOrDefaultAsync(x => x.Id == vehicalId);

                if (vehical == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Not found vehical";
                    return responeInfo;
                }

                vehical.LicensePlate = input.LicensePlate;
                vehical.Description = input.Description;

                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Message = "Update Vehical Success";
                responeInfo.Data.Add("vehical", vehical.Id);
                _logger.LogInformation("Update vehical success");
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