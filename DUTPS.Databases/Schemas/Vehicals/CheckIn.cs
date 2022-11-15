using System;
using DUTPS.Databases.Schemas.Authentication;

namespace DUTPS.Databases.Schemas.Vehicals
{
    public class CheckIn : IdentityTable
    {
        public DateTime DateOfCheckIn { get; set; }

        public bool IsCheckOut { get; set; }
        
        public long CustomerId { get; set; }
        
        public long StaffId { get; set; }

        public long VehicalId { get; set; }

        public virtual User Customer { get; set; }

        public virtual User Staff { get; set; }

        public virtual Vehical Vehical { get; set; }

        public virtual CheckOut CheckOut { get; set; }
    }
}