namespace Readerover.Domain.Common.Models;

public interface IAuditableEntity : IEntity
{
    DateTime CreatedDate { get; set; }

    DateTime? ModifiedDate { get; set; }
}
