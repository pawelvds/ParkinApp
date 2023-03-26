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
        RuleFor(x => x.UserTimeZoneId).NotEmpty();
    }
}
