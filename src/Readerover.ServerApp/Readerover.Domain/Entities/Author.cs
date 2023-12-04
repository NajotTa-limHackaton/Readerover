using Readerover.Domain.Common.Models;

namespace Readerover.Domain.Entities;

public class Author : AuditableEntity
{
    public string FullName { get; set; } = default!;

    public DateTime BirthDate { get; set; }

    public string Country { get; set; } = default!;
    
    public string? ImageUrl { get; set; } = default!;
}
