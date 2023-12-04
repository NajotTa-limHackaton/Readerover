namespace Readerover.Domain.Common.Models;

public interface ISoftDeletedEntity : IEntity
{
    bool IsDeleted { get; set; } 

    DateTime? DeletedDate { get; set; }
}
