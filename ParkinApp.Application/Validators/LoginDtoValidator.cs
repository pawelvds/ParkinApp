using FluentValidation;
using ParkinApp.Domain.DTOs;
using ParkinApp.DTOs;

namespace ParkinApp.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.UserTimeZoneId)
            .NotEmpty().WithMessage("Time zone is required.")
            .Must(IsValidTimeZoneId).WithMessage("Invalid time zone.");
    }
    
    private bool IsValidTimeZoneId(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
    }
}
