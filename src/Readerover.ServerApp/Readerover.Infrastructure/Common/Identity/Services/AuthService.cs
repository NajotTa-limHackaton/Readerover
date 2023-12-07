using Mapster;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Registering;
using Readerover.Domain.Entities;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class AuthService(
    IAccountService accountService,
    IUserService userService,
    IPasswordHasherService passwordHasherService,
    IAccessTokenGeneratorService tokenGeneratorService) : IAuthService
{
    public ValueTask<string> SignInAsync(LoginDetails loginDetails, CancellationToken cancellationToken = default)
    {
        var user = userService.Get(user => user.EmailAddress == loginDetails.EmailAddress).SingleOrDefault()
            ?? throw new InvalidOperationException("Username or password incorrent");

        if(!passwordHasherService.Verify(loginDetails.Password, user.Password))
            throw new InvalidOperationException("Username or password is incorrect");

        var token = tokenGeneratorService.GetToken(user);

        return new(token);
    }

    public async ValueTask<bool> SignUpAsync(RegisterDetails registerDetails, CancellationToken cancellationToken = default)
    {
        var res = await accountService.CreateAsync(registerDetails.Adapt<User>(), cancellationToken: cancellationToken);

        return true;
    }
}
