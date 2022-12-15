namespace DUTPS.API.Dtos.Vehicals
{
    public class CheckOutCreateDto
    {
        public DateTime DateOfCheckOut { get; set; }
        public long CheckInId { get; set; }
    }
}