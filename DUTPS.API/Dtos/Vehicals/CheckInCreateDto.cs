using System.ComponentModel.DataAnnotations;

namespace DUTPS.API.Dtos.Vehicals
{
    public class CheckInCreateDto
    {
        [Required]
        public long CustomerId { get; set; }
        
        [Required]
        public long VehicalId { get; set; }
        
        [Required]
        public DateTime DateOfCheckIn { get; set; }
    }
}