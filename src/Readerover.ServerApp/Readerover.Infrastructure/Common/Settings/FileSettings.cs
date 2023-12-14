namespace Readerover.Infrastructure.Common.Settings;

public class FileSettings
{
    public List<string> ValidProfileImageExtensions { get; set; } = new();

    public int MaxProfileImageSizeInBytes { get; set; }

    public List<string> ValidBookTypes { get; set; } = new();

    public int MaxBookSizeInBytes { get; set; }

    public List<string> ValidBookAudioType { get; set; } = new();

    public int MaxBookAudioSizeInBytes { get;set; }
}
