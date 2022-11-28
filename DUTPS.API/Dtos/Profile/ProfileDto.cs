using DUTPS.API.Dtos.Vehicals;

namespace DUTPS.API.Dtos.Profile
{
    public class ProfileDto
    {
        public long Id { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }
        
        public string Email { get; set; }
        
        public int Role { get; set; }

        public string Name { get; set; }
        
        public int? Gender { get; set; }
        
        public DateTime? Birthday { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Class { get; set; }
        
        public string FacultyId { get; set; }
        
        public string FalcultyName { get; set; }

        public List<VehicalDto> Vehicals { get; set; }
    }
}