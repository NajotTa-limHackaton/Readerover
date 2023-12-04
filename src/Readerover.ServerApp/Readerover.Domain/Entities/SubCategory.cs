using Readerover.Domain.Common.Models;

namespace Readerover.Domain.Entities;

public class SubCategory : AuditableEntity
{
    public string Name { get; set; } = default!;

    public Guid CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}
