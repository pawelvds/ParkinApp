using Microsoft.Extensions.Caching.Memory;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            var user = await _userRepository.GetUserByUsernameAsync(userId);

            var userActiveReservation = await _reservationRepository.GetActiveReservationByUserIdAsync(user.Id);
            if (userActiveReservation != null)
            {
                return Result<ReservationResultDto>.Failure("User already has an active reservation.");
            }

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

            var utcNow = DateTimeOffset.UtcNow;
            var reservationEndTime = new DateTimeOffset(utcNow.Date.AddDays(1).AddTicks(-1), TimeSpan.Zero);

            var reservation = new Reservation
            {
                UserId = user.Id,
                ParkingSpotId = parkingSpot.Id,
                CreatedReservationTime = utcNow,
                ReservationEndTime = reservationEndTime
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
            var user = await _userRepository.GetUserByUsernameAsync(userId);

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
    
            // Refresh cache for the parking spot after cancelling the reservation
            await RefreshParkingSpotCacheAsync(reservation.ParkingSpotId);

            return Result<string>.Success("Reservation cancelled.");
        }
        
        public async Task<Result<OccupiedParkingSpotDto>> GetOccupiedParkingSpotAsync(int parkingSpotId)
        {
            var parkingSpot = await GetParkingSpotWithReservationsByIdAsync(parkingSpotId);
            if (parkingSpot == null)
            {
                return Result<OccupiedParkingSpotDto>.Failure("Parking spot not found.");
            }

            var activeReservation = parkingSpot.Reservations.FirstOrDefault();

            if (activeReservation == null)
            {
                return Result<OccupiedParkingSpotDto>.Failure("Parking spot is not reserved.");
            }

            var user = await _userRepository.GetByIdAsync(activeReservation.UserId);
            if (user == null)
            {
                return Result<OccupiedParkingSpotDto>.Failure("User not found.");
            }

            var occupiedParkingSpotDto = new OccupiedParkingSpotDto(parkingSpotId, user.Login);
            return Result<OccupiedParkingSpotDto>.Success(occupiedParkingSpotDto);
        }


        private async Task RefreshParkingSpotCacheAsync(int parkingSpotId)
        {
            var parkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(parkingSpotId);
            UpdateParkingSpotCache(parkingSpot);
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

        private async Task<ParkingSpot?> GetParkingSpotWithReservationsByIdAsync(int parkingSpotId)
        {
            if (!_cache.TryGetValue(GetParkingSpotCacheKey(parkingSpotId), out ParkingSpot? parkingSpot))
            {
                parkingSpot = await _parkingSpotRepository
                    .GetQueryable()
                    .Include(ps => ps.Reservations)
                    .FirstOrDefaultAsync(ps => ps.Id == parkingSpotId);
                _cache.Set(GetParkingSpotCacheKey(parkingSpotId), parkingSpot);
            }

            return parkingSpot;
        }


    }

}

