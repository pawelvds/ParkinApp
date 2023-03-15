namespace ParkingApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public int? ReservedSpotId { get; set; }
        public ParkingSpot ReservedSpot { get; set; }
        public string UserTimeZoneId { get; set; }
    }
}