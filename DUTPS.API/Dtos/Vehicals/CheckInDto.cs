namespace DUTPS.API.Dtos.Vehicals
{
    public class CheckInDto
    {
        public long Id { get; set; }
        
        public long StaffId { get; set; }

        public long CustomerId { get; set; }
        
        public long VehicalId { get; set; }

        public string StaffName { get; set; }

        public string CustomerName { get; set; }

        public string VehicalLicensePlate { get; set; }

        public string VehicalDescription { get; set; }

        public bool IsCheckOut { get; set; }
        
        public DateTime DateOfCheckIn { get; set; }
    }
}