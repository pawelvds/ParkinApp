using FluentValidation;
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
        private readonly IValidator<CreateReservationDto> _createReservationValidator;
        private readonly IValidator<ParkingSpot> _parkingSpotValidator;

        public ReservationService(IParkingSpotRepository parkingSpotRepository, IUserRepository userRepository,
            IValidator<CreateReservationDto> createReservationValidator, IValidator<ParkingSpot> parkingSpotValidator)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _userRepository = userRepository;
            _createReservationValidator = createReservationValidator;
            _parkingSpotValidator = parkingSpotValidator;
        }

        public async Task<Result<ReservationResultDto>> CreateReservationAsync(CreateReservationDto reservationDto,
            string userId)
        {
            var validationResult = await _createReservationValidator.ValidateAsync(reservationDto);
            if (!validationResult.IsValid)
            {
                return Result<ReservationResultDto>.Failure(
                    validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var user = await _userRepository.GetUserByUsername(userId);
            if (user == null)
            {
                return Result<ReservationResultDto>.Failure("User not found.");
            }

            var parkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(reservationDto.ParkingSpotId);
            if (parkingSpot == null)
            {
                return Result<ReservationResultDto>.Failure("Parking spot not found.");
            }

            var parkingSpotValidationResult = await _parkingSpotValidator.ValidateAsync(parkingSpot);
            if (!parkingSpotValidationResult.IsValid)
            {
                return Result<ReservationResultDto>.Failure(parkingSpotValidationResult.Errors
                    .Select(x => x.ErrorMessage).ToList());
            }

            if (parkingSpot.IsReserved)
            {
                return Result<ReservationResultDto>.Failure("Parking spot is already reserved.");
            }

            parkingSpot.IsReserved = true;
            parkingSpot.UserId = user.Id;

            var userTimeZoneId = user.UserTimeZoneId;

            var spotTimeZone = TimeZoneInfo.FindSystemTimeZoneById(parkingSpot.SpotTimeZoneId);
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, spotTimeZone);
            parkingSpot.ReservationTime = localNow;

            DateTime reservationEndTime =
                new DateTime(localNow.Year, localNow.Month, localNow.Day, 23, 59, 59, DateTimeKind.Local);
            reservationEndTime = TimeZoneInfo.ConvertTime(reservationEndTime, spotTimeZone);
            parkingSpot.ReservationEndTime = reservationEndTime;

            parkingSpot.UserTimeZoneId = userTimeZoneId;

            user.ReservedSpotId = parkingSpot.Id;

            await _parkingSpotRepository.UpdateAsync(parkingSpot);
            await _userRepository.UpdateAsync(user);

            var reservationResultDto = new ReservationResultDto(
                parkingSpot.Id,
                user.Id,
                parkingSpot.ReservationTime.Value,
                parkingSpot.ReservationEndTime.Value
            );

            return Result<ReservationResultDto>.Success(reservationResultDto);
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

            parkingSpot.IsReserved = false;
            parkingSpot.UserId = null;
            parkingSpot.ReservationTime = null;
            parkingSpot.ReservationEndTime = null;
            parkingSpot.UserTimeZoneId = parkingSpot.SpotTimeZoneId;

            await _parkingSpotRepository.UpdateAsync(parkingSpot);

            user.ReservedSpotId = null;
            await _userRepository.UpdateAsync(user);

            return Result<string>.Success("Reservation cancelled.");
        }
    }

}