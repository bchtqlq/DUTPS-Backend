using System.ComponentModel.DataAnnotations;

namespace DUTPS.API.Dtos.Profile
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]        
        public string NewPassword { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }
    }
}