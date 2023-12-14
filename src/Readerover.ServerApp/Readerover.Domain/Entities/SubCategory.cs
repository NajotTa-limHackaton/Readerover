using Readerover.Domain.Common.Models;
using System.Text.Json.Serialization;

namespace Readerover.Domain.Entities;

public class SubCategory : AuditableEntity
{
    public string Name { get; set; } = default!;

    public Guid CategoryId { get; set; }

    [JsonIgnore]
    public virtual Category? Category { get; set; }

    [JsonIgnore]
    public virtual List<Book> Books { get; set; } = new();
}
