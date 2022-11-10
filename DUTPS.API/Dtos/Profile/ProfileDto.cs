namespace DUTPS.API.Dtos.Profile
{
    public class ProfileDto
    {
        public string Name { get; set; }
        
        public int? Gender { get; set; }
        
        public DateTime? Birthday { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Class { get; set; }
        
        public string FacultyId { get; set; }
        
        public string FalcultyName { get; set; }
    }
}