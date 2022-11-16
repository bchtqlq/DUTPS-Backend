using DUTPS.API.Dtos.Vehicals;
using DUTPS.Commons;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using DUTPS.Databases;
using DUTPS.Databases.Schemas.Vehicals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DUTPS.API.Extensions;
namespace DUTPS.API.Services
{
    public interface ICheckInService
    {
        Task<ResponseInfo> CreateCheckIn(string staffUsername, CheckInCreateDto checkInCreateDto);
        Task<CheckInDto> GetCurrentCheckInByUsername(string customerUsername);
        Task<CheckInDto> GetCheckInByCheckInId(long checkInId);
        Task<ResponseInfo> CheckOut(string staffUsername, CheckOutCreateDto checkOutCreateDto);
        Task<PaginatedList<CheckInDto>> GetAvailableCheckIn(AvailableCheckInSearchCondition condition);
        Task<PaginatedList<CheckInHistoryDto>> GetCheckInsHistory(CheckInHistorySearchCondition condition);
    }
    public class CheckInService : ICheckInService
    {
        private readonly DataContext _context;
        private readonly ILogger<CheckInService> _logger;

        public CheckInService(
            DataContext context,
            ILogger<CheckInService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseInfo> CheckOut(string staffUsername, CheckOutCreateDto checkOutCreateDto)
        {
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation("Begin create check out");
                var responeInfo = new ResponseInfo();
                staffUsername = staffUsername.ToLower();
                var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == staffUsername);

                if (currentUser == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Not found user";
                    return responeInfo;
                }

                var checkOut = new CheckOut();
                 
                checkOut.StaffId = currentUser.Id;
                checkOut.DateOfCheckOut = checkOutCreateDto.DateOfCheckOut.AddHours(7);
                checkOut.CheckInId = checkOutCreateDto.CheckInId;

                await _context.CheckOuts.AddAsync(checkOut);
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();

                var checkIn = await _context.CheckIns.FirstOrDefaultAsync(x => x.Id == checkOutCreateDto.CheckInId);

                if (checkIn == null)
                {
                    throw new Exception("Check out is Invalid");
                }
                checkIn.IsCheckOut = true;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Message = "Create check out Success";
                responeInfo.Data.Add("checkInId", checkIn.Id);
                _logger.LogInformation("Create check out success");
                return responeInfo;
            }
            catch (Exception e)
            {
                await DataContext.RollbackAsync(transaction);
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }

        public async Task<ResponseInfo> CreateCheckIn(string staffUsername, CheckInCreateDto checkInCreateDto)
        {
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation("Begin create checkIn");
                var responeInfo = new ResponseInfo();
                staffUsername = staffUsername.ToLower();
                var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == staffUsername);

                if (currentUser == null)
                {
                    responeInfo.Code = CodeResponse.NOT_FOUND;
                    responeInfo.Message = "Not found user";
                    return responeInfo;
                }

                var checkIn = new CheckIn();
                 
                checkIn.StaffId = currentUser.Id;
                checkIn.CustomerId = checkInCreateDto.CustomerId;
                checkIn.DateOfCheckIn = checkInCreateDto.DateOfCheckIn.AddHours(7);
                checkIn.VehicalId = checkInCreateDto.VehicalId;
                checkIn.IsCheckOut = false;

                await _context.CheckIns.AddAsync(checkIn);
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                responeInfo.Message = "Create check in Success";
                responeInfo.Data.Add("checkInId", checkIn.Id);
                _logger.LogInformation("Create check in success");
                return responeInfo;
            }
            catch (Exception e)
            {
                await DataContext.RollbackAsync(transaction);
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<CheckInDto>> GetAvailableCheckIn(AvailableCheckInSearchCondition condition)
        {
            try
            {
                _logger.LogInformation("Start get available check in list");
                if (condition == null)
                {
                    condition = new AvailableCheckInSearchCondition();
                }
                var checkIns = new PaginatedList<CheckInDto>();

                var query = _context.CheckIns
                    .Where(x => !x.IsCheckOut)
                    .Where(x => 
                        (String.IsNullOrEmpty(condition.Query) || 
                            x.Vehical.LicensePlate.ToLower().Contains(condition.Query.ToLower())||
                            x.Customer.Information.Name.ToLower().Contains(condition.Query.ToLower())));
                
                checkIns.Paging = new Paging(await query.CountAsync(), condition.Page, condition.Limit);
                checkIns.List = await query
                    .OrderBy(condition)
                    .Skip((checkIns.Paging.Page - 1) * checkIns.Paging.Limit)
                    .Take(checkIns.Paging.Limit)
                    .Select(x => new CheckInDto {
                        DateOfCheckIn = x.DateOfCheckIn,
                        VehicalId = x.VehicalId,
                        VehicalLicensePlate = x.Vehical.LicensePlate,
                        VehicalDescription = x.Vehical.Description,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer.Information.Name,
                        StaffName = x.Staff.Information.Name,
                    }).ToListAsync();
                if (!checkIns.List.Any())
                {
                    checkIns.Paging.Total = 0;
                }
                _logger.LogInformation("End get list history");
                return checkIns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CheckInDto> GetCheckInByCheckInId(long checkInId)
        {
            try
            {
                _logger.LogInformation("Begin create checkIn");

                var checkIn = await _context.CheckIns
                    .Select(x => new CheckInDto
                    {
                        Id = x.Id,
                        StaffId = x.StaffId,
                        StaffName = x.Staff.Information.Name,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer.Information.Name,
                        VehicalId = x.VehicalId,
                        VehicalLicensePlate = x.Vehical.LicensePlate,
                        VehicalDescription = x.Vehical.Description,
                        DateOfCheckIn = x.DateOfCheckIn,
                        IsCheckOut = x.IsCheckOut
                    })
                    .FirstOrDefaultAsync(x => x.Id == checkInId);
                _logger.LogInformation("Get check in success");

                return checkIn;
            }
            catch (Exception e)
            {
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }

        private static DateTime GetValidDatetime(DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public async Task<PaginatedList<CheckInHistoryDto>> GetCheckInsHistory(CheckInHistorySearchCondition condition)
        {
            try
            {
                _logger.LogInformation("Start get check in history list");
                if (condition == null)
                {
                    condition = new CheckInHistorySearchCondition();
                }
                var checkIns = new PaginatedList<CheckInHistoryDto>();

                if (condition.CheckInBeginDate.HasValue)
                {
                    condition.CheckInBeginDate = GetValidDatetime(condition.CheckInBeginDate.Value);
                }

                if (condition.CheckInEndDate.HasValue)
                {
                    condition.CheckInEndDate = GetValidDatetime(condition.CheckInEndDate.Value);
                }

                var query = _context.CheckOuts
                    .Where(x => x.CheckIn.IsCheckOut)
                    .Where(x => 
                        (String.IsNullOrEmpty(condition.Query) || 
                            x.CheckIn.Vehical.LicensePlate.ToLower().Contains(condition.Query.ToLower())||
                            x.CheckIn.Customer.Information.Name.ToLower().Contains(condition.Query.ToLower()))  &&
                        (!condition.CheckInBeginDate.HasValue || x.CheckIn.DateOfCheckIn.Date >= condition.CheckInBeginDate.Value.Date) &&
                        (!condition.CheckInEndDate.HasValue || x.CheckIn.DateOfCheckIn.Date <= condition.CheckInEndDate.Value.Date));
                
                checkIns.Paging = new Paging(await query.CountAsync(), condition.Page, condition.Limit);
                checkIns.List = await query
                    .OrderBy(condition)
                    .Skip((checkIns.Paging.Page - 1) * checkIns.Paging.Limit)
                    .Take(checkIns.Paging.Limit)
                    .Select(x => new CheckInHistoryDto {
                        DateOfCheckIn = x.CheckIn.DateOfCheckIn,
                        DateOfCheckOut = x.DateOfCheckOut,
                        VehicalId = x.CheckIn.VehicalId,
                        VehicalLicensePlate = x.CheckIn.Vehical.LicensePlate,
                        VehicalDescription = x.CheckIn.Vehical.Description,
                        CustomerId = x.CheckIn.CustomerId,
                        CustomerName = x.CheckIn.Customer.Information.Name,
                        StaffCheckInName = x.CheckIn.Staff.Information.Name,
                        StaffCheckOutName = x.Staff.Information.Name
                    }).ToListAsync();
                if (!checkIns.List.Any())
                {
                    checkIns.Paging.Total = 0;
                }
                _logger.LogInformation("End get list history");
                return checkIns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CheckInDto> GetCurrentCheckInByUsername(string customerUsername)
        {
            try
            {
                _logger.LogInformation("Begin create checkIn");
                customerUsername = customerUsername.ToLower();
                var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == customerUsername);

                if (currentUser == null)
                {
                    return new CheckInDto();
                }

                var checkIn = await _context.CheckIns
                    .Select(x => new CheckInDto
                    {
                        Id = x.Id,
                        StaffId = x.StaffId,
                        StaffName = x.Staff.Information.Name,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer.Information.Name,
                        VehicalId = x.VehicalId,
                        VehicalLicensePlate = x.Vehical.LicensePlate,
                        VehicalDescription = x.Vehical.Description,
                        DateOfCheckIn = x.DateOfCheckIn,
                        IsCheckOut = x.IsCheckOut
                    })
                    .FirstOrDefaultAsync(x => x.CustomerId == currentUser.Id && !x.IsCheckOut);
                _logger.LogInformation("Get check in success");

                return checkIn;
            }
            catch (Exception e)
            {
                _logger.LogError($"Has the follow erorr {e.Message}");
                throw;
            }
        }
    }
}