using ParkinApp.DTOs;
using ParkinApp.Domain.Common;
using System.Threading.Tasks;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IReservationService
    {
        Task<Result<ParkingSpot>> CreateReservationAsync(CreateReservationDto reservationDto, string userId);

        Task<Result<string>> CancelReservationAsync(string userId);
    }
}