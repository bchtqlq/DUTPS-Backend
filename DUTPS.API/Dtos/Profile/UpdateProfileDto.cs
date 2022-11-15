using System.ComponentModel.DataAnnotations;

namespace DUTPS.API.Dtos.Profile
{
    public class UpdateProfileDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public int? Gender { get; set; }
        
        public DateTime? Birthday { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Class { get; set; }
        
        [Required]
        public string FacultyId { get; set; }        
    }
}