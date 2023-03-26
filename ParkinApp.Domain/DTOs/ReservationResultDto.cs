namespace ParkinApp.Domain.DTOs
{
    public record ReservationResultDto(int ParkingSpotId, int UserId, DateTime ReservationTime, DateTime ReservationEndTime);
}