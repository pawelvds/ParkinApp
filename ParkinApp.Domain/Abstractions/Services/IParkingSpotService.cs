using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkinApp.Domain.Abstractions.Services
{
    public interface IParkingSpotService
    {
        Task<List<ParkingSpotDto>> GetAllParkingSpotsAsync();
    }

    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IParkingSpotRepository _parkingSpotRepository;

        public ParkingSpotService(IParkingSpotRepository parkingSpotRepository)
        {
            _parkingSpotRepository = parkingSpotRepository;
        }

        public async Task<List<ParkingSpotDto>> GetAllParkingSpotsAsync()
        {
            var parkingSpots = await _parkingSpotRepository.GetAllAsync();
            return parkingSpots.Select(ps =>
            {
                var activeReservation = ps.Reservations.FirstOrDefault(r => r.ReservationEndTime > DateTimeOffset.UtcNow);
                var reservedBy = activeReservation?.User?.Login;
                var reserved = activeReservation != null;

                return new ParkingSpotDto(ps.Id, ps.SpotTimeZone, reserved, reservedBy);
            }).ToList();
        }
    }
}