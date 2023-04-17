namespace ParkinApp.DTOs
{
    public record ParkingSpotDto(int Id, string SpotTimeZone, bool Reserved, string? ReservedBy);
}