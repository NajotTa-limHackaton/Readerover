using Readerover.Application.Common.Models.Registering;
using Readerover.Domain.Entities;

namespace Readerover.Application.Common.Identity.Services;

public interface IAuthService
{
    ValueTask<bool> SignUpAsync(RegisterDetails registerDetails, CancellationToken cancellationToken = default);

    ValueTask<string> SignInAsync(LoginDetails loginDetails, CancellationToken cancellationToken = default);
}
