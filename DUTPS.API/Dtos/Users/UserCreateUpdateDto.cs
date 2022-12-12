using System.ComponentModel.DataAnnotations;

namespace DUTPS.API.Dtos.Users
{
    public class UserCreateUpdateDto
    {
        [Required]
        [StringLength(512)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(512)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string ConfirmPassword { get; set; }

        [Required]
        public int Role { get; set; }
        
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