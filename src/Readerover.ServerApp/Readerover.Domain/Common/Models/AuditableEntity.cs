namespace Readerover.Domain.Common.Models;

public class AuditableEntity : SoftDeletedEntity, IAuditableEntity
{
    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
