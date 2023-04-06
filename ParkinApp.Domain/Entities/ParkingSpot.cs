namespace ParkinApp.Domain.Entities;

public class ParkingSpot
{
    public int Id { get; set; }
    public string SpotTimeZone { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    
}