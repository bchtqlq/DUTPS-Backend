using System.ComponentModel.DataAnnotations;

namespace DUTPS.API.Dtos.Vehicals
{
    public class VehicalCreateUpdateDto
    {
        [Required]
        [StringLength(16)]
        public string LicensePlate { get; set; }
        
        public string Description { get; set; }
    }
}