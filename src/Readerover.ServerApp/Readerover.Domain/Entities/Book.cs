using Readerover.Domain.Common.Models;

namespace Readerover.Domain.Entities;

public class Book : AuditableEntity
{
    public string Name { get; set; } = default!;

    public Guid CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public Guid SubCategoryId { get; set; }

    public virtual SubCategory? SubCategory { get; set; }

    public Guid AuthorId { get; set; }

    public virtual Author? Author { get; set;}

    public string BookUrl { get; set; }

    public string? AudioUrl { get; set; }
}
