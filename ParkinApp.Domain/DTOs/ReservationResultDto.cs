namespace ParkinApp.Domain.DTOs
{
    public record ReservationResultDto(int ParkingSpotId, int UserId, DateTimeOffset ReservationTime, DateTimeOffset ReservationEndTime);
}