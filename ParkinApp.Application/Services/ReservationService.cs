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
        private readonly IParkingSpotCacheService _parkingSpotCacheService;

        public ReservationService(IParkingSpotRepository parkingSpotRepository, IUserRepository userRepository,
            IReservationRepository reservationRepository, IParkingSpotCacheService parkingSpotCacheService)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
            _parkingSpotCacheService = parkingSpotCacheService;
        }

        public async Task<Result<ReservationResultDto>> CreateReservationAsync(CreateReservationDto reservationDto,
            string userId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userId);

            var userActiveReservation = await _reservationRepository.GetActiveReservationByUserIdAsync(user.Id);
            if (userActiveReservation != null)
            {
                return Result<ReservationResultDto>.Failure("User already has an active reservation.");
            }

            var parkingSpot =
                await _parkingSpotCacheService.GetParkingSpotByIdAsync(reservationDto.ParkingSpotId,
                    _parkingSpotRepository);
            if (parkingSpot == null)
            {
                return Result<ReservationResultDto>.Failure("Parking spot not found.");
            }

            var activeReservation =
                parkingSpot.Reservations.FirstOrDefault(r => r.ReservationEndTime > DateTimeOffset.UtcNow);
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

            _parkingSpotCacheService.UpdateParkingSpotCache(parkingSpot);

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
            await _parkingSpotCacheService.RefreshParkingSpotCacheAsync(reservation.ParkingSpotId,
                _parkingSpotRepository);

            return Result<string>.Success("Reservation cancelled.");
        }

        public async Task<Result<ReservationResultDto>> GetCurrentReservationAsync(string userId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userId);

            if (user == null)
            {
                return Result<ReservationResultDto>.Failure("User not found.");
            }

            var reservation = await _reservationRepository.GetActiveReservationByUserIdAsync(user.Id);
            if (reservation == null)
            {
                return Result<ReservationResultDto>.Failure("User doesn't have any active reservation.");
            }

            var parkingSpot =
                await _parkingSpotCacheService.GetParkingSpotByIdAsync(reservation.ParkingSpotId,
                    _parkingSpotRepository);

            var reservationResultDto = new ReservationResultDto(
                parkingSpot.Id,
                user.Id,
                reservation.CreatedReservationTime,
                reservation.ReservationEndTime
            );

            return Result<ReservationResultDto>.Success(reservationResultDto);
        }
    }
}

