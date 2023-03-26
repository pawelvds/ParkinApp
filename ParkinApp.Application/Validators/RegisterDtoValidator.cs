using FluentValidation;
using ParkinApp.Domain.DTOs;
using ParkinApp.DTOs;

namespace ParkinApp.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
