using System;
using DUTPS.Databases.Schemas.Authentication;

namespace DUTPS.Databases.Schemas.Vehicals
{
    public class CheckOut : IdentityTable
    {
        public DateTime DateOfCheckOut { get; set; }
        
        public long StaffId { get; set; }
        
        public long CheckInId { get; set; }
        
        public virtual User Staff { get; set; }
        
        public virtual CheckIn CheckIn { get; set; }
    }
}