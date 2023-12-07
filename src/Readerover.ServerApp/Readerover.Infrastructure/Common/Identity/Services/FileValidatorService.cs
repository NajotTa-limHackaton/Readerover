using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Exceptions;
using Readerover.Infrastructure.Common.Settings;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class FileValidatorService(IOptions<FileSettings> fileOptions) : IFileValidatorService
{
    private readonly FileSettings _fileOptions = fileOptions.Value;

    public ValueTask<bool> ValidateProfileImage(IFormFile image, CancellationToken cancellationToken = default)
    {
        var extension = image.FileName.Split('.').Last();

        if (!_fileOptions.ValidProfileImageExtensions.Contains(extension))
            throw new FileValidationException("Image file type not supported!");

        if (image.Length > _fileOptions.MaxProfileImageSizeInBytes)
            throw new FileValidationException("The image size is large!");

        return new(true);
    }
}
