using FluentValidation;
using ParkinApp.Domain.Entities;

namespace ParkinApp.Validators;

public class ParkingSpotValidator : AbstractValidator<ParkingSpot>
{
    public ParkingSpotValidator()
    {
        RuleFor(x => x.SpotTimeZone).NotEmpty().WithMessage("SpotTimeZone is required.");
    }
}