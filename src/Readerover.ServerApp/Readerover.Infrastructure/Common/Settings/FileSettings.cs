namespace Readerover.Infrastructure.Common.Settings;

public class FileSettings
{
    public List<string> ValidProfileImageExtensions { get; set; } = new();

    public int MaxProfileImageSizeInBytes { get; set; }
}
