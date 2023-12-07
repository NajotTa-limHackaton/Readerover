using Readerover.Domain.Common.Enums;
using Readerover.Domain.Common.Models;

namespace Readerover.Domain.Entities;

public class StorageFile : AuditableEntity
{
    public string FileName { get; set; } = default!;

    public FileType Type { get; set; }

    public string Extension { get; set; } = default!;

    public string FilePath { get; set; } = default!;
}
