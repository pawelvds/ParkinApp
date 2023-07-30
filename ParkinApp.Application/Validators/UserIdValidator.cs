using FluentValidation;
using ParkinApp.Domain.Abstractions.Repositories;

namespace ParkinApp.Validators
{
    public class UserIdValidator : AbstractValidator<string>
    {
        public UserIdValidator(IUserRepository userRepository)
        {
            RuleFor(userId => userId)
                .MustAsync(async (userId, cancellationToken) =>
                {
                    var user = await userRepository.GetUserByUsernameAsync(userId);
                    return user != null;
                })
                .WithMessage("User not found.");
        }
    }
}