using DUTPS.API.Dtos.Commons;
using DUTPS.Databases;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.API.Services
{
    public interface ICommonService
    {
        Task<List<FacultyDto>> GetFaculties();
    }
    public class CommonService : ICommonService
    {
        private readonly DataContext _context;

        public CommonService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<FacultyDto>> GetFaculties()
        {
            return await _context.Faculties.Select(x => new FacultyDto {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
        }
    }
}