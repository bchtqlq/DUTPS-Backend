using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DUTPS.Databases.Schemas.Authentication;

namespace DUTPS.Databases.Schemas.Vehicals
{
    public class Vehical : IdentityTable
    {
        public Vehical()
        {
            CheckIns = new HashSet<CheckIn>();
        }
        
        [Required]
        [StringLength(16)]
        public string LicensePlate { get; set; }
        
        [StringLength(512)]
        public string Description { get; set; }
        
        public long CustomerId { get; set; }
        
        public virtual User Customer { get; set; }

        public virtual HashSet<CheckIn> CheckIns { get; set; }
    }
}