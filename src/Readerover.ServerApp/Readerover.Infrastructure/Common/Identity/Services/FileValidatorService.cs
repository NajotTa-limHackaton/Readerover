using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Exceptions;
using Readerover.Infrastructure.Common.Settings;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class FileValidatorService(IOptions<FileSettings> fileOptions) : IFileValidatorService
{
    private readonly FileSettings _fileOptions = fileOptions.Value;

    public ValueTask<bool> DeleteBookAudioFileIfExist(string bookPath, CancellationToken cancellationToken = default)
    {
        foreach (var extension in _fileOptions.ValidBookAudioType)
        {
            var bookFilePath = Path.Combine(bookPath, $"audio.{extension}");
            if (File.Exists(bookFilePath))
            {
                File.Delete(bookFilePath);
                return new(true);
            }
        }
        return new(false);
    }

    public ValueTask<bool> DeleteBookDocumentIfExist(string bookPath, CancellationToken cancellationToken = default)
    {
        foreach (var extension in _fileOptions.ValidBookTypes)
        {
            var bookFilePath = Path.Combine(bookPath, $"document.{extension}");
            if (File.Exists(bookFilePath))
            {
                File.Delete(bookFilePath);
                return new(true);
            }
        }
        return new(false);
    }

    public ValueTask<bool> DeleteIfImageExistsAsync(Guid userId,string folderName, string webRootPath, CancellationToken cancellationToken)
    {
        var absolutePath = Path.Combine(webRootPath, folderName);

        foreach (var extension in _fileOptions.ValidProfileImageExtensions)
        {
            var imagePath = Path.Combine(absolutePath, $"{userId}.{extension}");

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
                return new(true);
            }
        }
        return new(false);
    }

    public ValueTask<bool> ValidateBookAudioAsync(IFormFile audio, CancellationToken cancellationToken = default)
    {
        var extension = audio.FileName.Split('.').Last();

        if (!_fileOptions.ValidBookAudioType.Contains(extension)) throw new FileValidationException("Only mp3 supported!");

        if (audio.Length > _fileOptions.MaxBookAudioSizeInBytes) throw new FileValidationException("Audio file is too large");

        return new(true);
    }

    public ValueTask<bool> ValidateBookFileAsync(IFormFile book, CancellationToken cancellationToken = default)
    {
        var extension = book.FileName.Split('.').Last();

        if (!_fileOptions.ValidBookTypes.Contains(extension))
            throw new FileValidationException("Book file type not supported!");

        if (book.Length > _fileOptions.MaxBookSizeInBytes)
            throw new FileValidationException("The book size is large!");

        return new(true);
    }

    public ValueTask<bool> ValidateImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var extension = image.FileName.Split('.').Last();

        if (!_fileOptions.ValidProfileImageExtensions.Contains(extension))
            throw new FileValidationException("Image file type not supported!");

        if (image.Length > _fileOptions.MaxProfileImageSizeInBytes)
            throw new FileValidationException("The image size is large!");

        return new(true);
    }
}
