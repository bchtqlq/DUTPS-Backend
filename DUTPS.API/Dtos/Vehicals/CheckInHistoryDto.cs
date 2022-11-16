namespace DUTPS.API.Dtos.Vehicals
{
    public class CheckInHistoryDto
    {
        public DateTime DateOfCheckIn { get; set; }
        
        public DateTime DateOfCheckOut { get; set; }

        public long VehicalId { get; set; }
        
        public string VehicalLicensePlate { get; set; }
        
        public string VehicalDescription { get; set; }
        
        public long CustomerId { get; set; }
        
        public string CustomerName { get; set; }

        public string StaffCheckInName { get; set; }
        
        public string StaffCheckOutName { get; set; }
    }
}