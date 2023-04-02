using FluentValidation;
using ParkinApp.Domain.DTOs;

namespace ParkinApp.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.UserTimeZoneId)
            .NotEmpty().WithMessage("Time zone is required.")
            .Must(IsValidTimeZoneId).WithMessage("Invalid time zone."); // date time offset now!!!!
    }
    
    private bool IsValidTimeZoneId(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException) // do przeróbki, wyjątek, nie bedzie stref czasowych
        {
            return false;
        }
    }
}
