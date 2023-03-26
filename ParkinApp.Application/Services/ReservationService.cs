using FluentValidation;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;
using ParkinApp.DTOs;

namespace ParkinApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IParkingSpotRepository _parkingSpotRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateReservationDto> _createReservationValidator;

        public ReservationService(IParkingSpotRepository parkingSpotRepository, IUserRepository userRepository,
            IValidator<CreateReservationDto> createReservationValidator)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _userRepository = userRepository;
            _createReservationValidator = createReservationValidator;
        }

        public async Task<Result<ParkingSpot>> CreateReservationAsync(CreateReservationDto reservationDto, string userId)
        {
            var user = await _userRepository.GetUserByUsername(userId);
            if (user == null)
            {
                return Result<ParkingSpot>.Failure("User not found.");
            }

            var parkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(reservationDto.ParkingSpotId);
            if (parkingSpot == null)
            {
                return Result<ParkingSpot>.Failure("Parking spot not found.");
            }

            if (parkingSpot.IsReserved)
            {
                return Result<ParkingSpot>.Failure("Parking spot is already reserved.");
            }

            parkingSpot.IsReserved = true;
            parkingSpot.UserId = user.Id;

            var userTimeZoneId = user.UserTimeZoneId;

            var spotTimeZone = TimeZoneInfo.FindSystemTimeZoneById(parkingSpot.SpotTimeZoneId);
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, spotTimeZone);
            parkingSpot.ReservationTime = localNow;

            DateTime reservationEndTime = new DateTime(localNow.Year, localNow.Month, localNow.Day, 23, 59, 59, DateTimeKind.Local);
            reservationEndTime = TimeZoneInfo.ConvertTime(reservationEndTime, spotTimeZone);
            parkingSpot.ReservationEndTime = reservationEndTime;

            parkingSpot.UserTimeZoneId = userTimeZoneId;

            user.ReservedSpotId = parkingSpot.Id;

            await _parkingSpotRepository.UpdateAsync(parkingSpot);
            await _userRepository.UpdateAsync(user);

            return Result<ParkingSpot>.Success(parkingSpot);
        }



        public async Task<Result<string>> CancelReservationAsync(string userId)
        {
            var user = await _userRepository.GetUserByUsername(userId);
            if (user == null)
            {
                return Result<string>.Failure("User not found.");
            }

            if (user.ReservedSpotId == null)
            {
                return Result<string>.Failure("User doesn't have any reserved spot.");
            }

            var parkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(user.ReservedSpotId.Value);
            if (parkingSpot == null)
            {
                return Result<string>.Failure("Reserved parking spot not found.");
            }

            // Update the parking spot to set IsReserved to false
            parkingSpot.IsReserved = false;
            parkingSpot.UserId = null;
            parkingSpot.ReservationTime = null;
            parkingSpot.ReservationEndTime = null;
            parkingSpot.UserTimeZoneId = parkingSpot.SpotTimeZoneId;

            // Save the updated parking spot to the database
            await _parkingSpotRepository.UpdateAsync(parkingSpot);

            // Update the user's reserved spot ID to null
            user.ReservedSpotId = null;
            await _userRepository.UpdateAsync(user);

            return Result<string>.Success("Reservation cancelled.");
        }

    }
}