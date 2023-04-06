namespace ParkinApp.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ParkingSpotId { get; set; }
    public ParkingSpot? ParkingSpot { get; set; }
    public DateTime ReservationEndTime { get; set; }
    public DateTime CreatedReservationTime { get; set; }
}
