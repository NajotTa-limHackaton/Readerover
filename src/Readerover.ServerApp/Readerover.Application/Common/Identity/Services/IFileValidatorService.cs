using Microsoft.AspNetCore.Http;

namespace Readerover.Application.Common.Identity.Services;

public interface IFileValidatorService
{
    ValueTask<bool> ValidateProfileImage(IFormFile image, CancellationToken cancellationToken = default);
}
