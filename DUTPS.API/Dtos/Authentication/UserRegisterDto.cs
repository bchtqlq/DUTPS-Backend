using System.ComponentModel.DataAnnotations;

namespace DUTPS.API.Dtos.Authentication
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(512)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(512)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string ConfirmPassword { get; set; }
    }
}
