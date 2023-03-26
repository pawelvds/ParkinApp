using FluentValidation;
using ParkinApp.Domain.DTOs;
using ParkinApp.DTOs;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(dto => dto.ParkingSpotId)
            .GreaterThan(0)
            .WithMessage("Invalid parking spot ID.");
    }
}