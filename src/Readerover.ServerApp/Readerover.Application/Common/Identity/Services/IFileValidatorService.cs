using Microsoft.AspNetCore.Http;

namespace Readerover.Application.Common.Identity.Services;

public interface IFileValidatorService
{
    ValueTask<bool> ValidateImageAsync(IFormFile image, CancellationToken cancellationToken = default);

    ValueTask<bool> ValidateBookFileAsync(IFormFile book, CancellationToken cancellationToken = default);

    ValueTask<bool> ValidateBookAudioAsync(IFormFile audio, CancellationToken cancellationToken = default);

    ValueTask<bool> DeleteIfImageExistsAsync(Guid userId, string folderName,string webRootPath, CancellationToken cancellationToken);

    ValueTask<bool> DeleteBookDocumentIfExist(string bookPath, CancellationToken cancellationToken = default);

    ValueTask<bool> DeleteBookAudioFileIfExist(string bookPath, CancellationToken cancellationToken = default);
}
