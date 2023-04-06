namespace ParkinApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string UserTimeZone { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}