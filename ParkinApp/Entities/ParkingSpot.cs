namespace ParkingApp.Entities;

public class ParkingSpot
{
    public int Id { get; set; }
    public bool IsReserved { get; set; }
    public DateTime? ReservationTime { get; set; }
    public DateTime? ReservationEndTime { get; set; }
    public string SpotTimeZoneId { get; set; } 
    public string UserTimeZoneId { get; set; } 
    public int? UserId { get; set; }
    public User User { get; set; }
}