using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IReservationService
    {
        Task<Result<ReservationResultDto>> CreateReservationAsync(CreateReservationDto reservationDto, string userId);
        
        Task<Result<string>> CancelReservationAsync(string userId);
        
        Task<Result<OccupiedParkingSpotDto>> GetOccupiedParkingSpotAsync(int parkingSpotId);
    }
}