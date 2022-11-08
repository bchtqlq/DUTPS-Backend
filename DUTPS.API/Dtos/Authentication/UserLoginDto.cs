using System.ComponentModel.DataAnnotations;
namespace DUTPS.API.Dtos.Authentication
{
    public class UserLoginDto
    {
        [Required]
        [StringLength(256)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(256)]
        public string Password { get; set; }
    }
}