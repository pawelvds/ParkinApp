using FluentValidation;
using ParkinApp.Domain.Entities;

public class ParkingSpotValidator : AbstractValidator<ParkingSpot>
{
    public ParkingSpotValidator()
    {
        RuleFor(x => x.SpotTimeZoneId).NotEmpty().WithMessage("SpotTimeZoneId is required.");
    }
}