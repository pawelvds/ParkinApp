using FluentValidation;

namespace ParkinApp.Validators;

public class GetParkingSpotsRequestValidator : AbstractValidator<object>
{
    public GetParkingSpotsRequestValidator()
    {
        RuleFor(x => x).NotNull();
    }
}
