using Microsoft.Extensions.Caching.Memory;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IParkingSpotRepository _parkingSpotRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMemoryCache _cache;

        private const string ParkingSpotCacheKeyPrefix = "ParkingSpot_";

        public ReservationService(IParkingSpotRepository parkingSpotRepository, IUserRepository userRepository, IReservationRepository reservationRepository, IMemoryCache cache)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
            _cache = cache;
        }

        public async Task<Result<ReservationResultDto>> CreateReservationAsync(CreateReservationDto reservationDto, string userId)
        {
            var user = await _userRepository.GetUserByUsername(userId);

            var parkingSpot = await GetParkingSpotByIdAsync(reservationDto.ParkingSpotId);
            if (parkingSpot == null)
            {
                return Result<ReservationResultDto>.Failure("Parking spot not found.");
            }

            var activeReservation = parkingSpot.Reservations.FirstOrDefault(r => r.ReservationEndTime > DateTimeOffset.UtcNow);
            if (activeReservation != null)
            {
                return Result<ReservationResultDto>.Failure("Parking spot is already reserved.");
            }

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
            var localDateTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZoneInfo);
            var reservationEndTime = localDateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var reservationEndTimeUtc = TimeZoneInfo.ConvertTime(reservationEndTime, timeZoneInfo);

            var reservation = new Reservation
            {
                UserId = user.Id,
                ParkingSpotId = parkingSpot.Id,
                CreatedReservationTime = DateTimeOffset.UtcNow,
                ReservationEndTime = reservationEndTimeUtc
            };

            await _reservationRepository.AddAsync(reservation);

            var reservationResultDto = new ReservationResultDto(
                parkingSpot.Id,
                user.Id,
                reservation.CreatedReservationTime,
                reservation.ReservationEndTime
            );

            UpdateParkingSpotCache(parkingSpot);

            return Result<ReservationResultDto>.Success(reservationResultDto);
        }



        public async Task<Result<string>> CancelReservationAsync(string userId)
        {
            var user = await _userRepository.GetUserByUsername(userId);

            if (user == null)
            {
                return Result<string>.Failure("User not found.");
            }
            
            var reservation = await _reservationRepository.GetActiveReservationByUserIdAsync(user.Id);
            if (reservation == null)
            {
                return Result<string>.Failure("User doesn't have any active reservation.");
            }

            await _reservationRepository.DeleteAsync(reservation);

            return Result<string>.Success("Reservation cancelled.");
        }

        private async Task<ParkingSpot?> GetParkingSpotByIdAsync(int parkingSpotId)
        {
            if (!_cache.TryGetValue(GetParkingSpotCacheKey(parkingSpotId), out ParkingSpot? parkingSpot))
            {
                parkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(parkingSpotId);
                _cache.Set(GetParkingSpotCacheKey(parkingSpotId), parkingSpot);
            }

            return parkingSpot;
        }

        private void UpdateParkingSpotCache(ParkingSpot? parkingSpot)
        {
            if (parkingSpot != null) _cache.Set(GetParkingSpotCacheKey(parkingSpot.Id), parkingSpot);
        }

        private string GetParkingSpotCacheKey(int parkingSpotId)
        {
            return $"{ParkingSpotCacheKeyPrefix}{parkingSpotId}";
        }
    }
}

