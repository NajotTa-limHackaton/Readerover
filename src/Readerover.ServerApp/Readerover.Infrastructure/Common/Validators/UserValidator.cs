using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Readerover.Domain.Entities;
using Readerover.Infrastructure.Common.Constants;
using System.Text.RegularExpressions;

namespace Readerover.Infrastructure.Common.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.FirstName).NotEmpty().MinimumLength(3).MaximumLength(32);

        RuleFor(user => user.LastName).NotEmpty().MinimumLength(3).MaximumLength(32);

        RuleFor(user => user.Country).NotEmpty().MinimumLength(5).MaximumLength(256);

        RuleFor(user => user.EmailAddress)
            .Custom((emailAddress, context) =>
            {
                if (!Regex.IsMatch(emailAddress, RegexConstants.EmailRegex))
                    context.AddFailure(nameof(emailAddress), "Invalid email address!");
            });
    }
}
