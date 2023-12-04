
namespace Readerover.Domain.Common.Models;

public class SoftDeletedEntity : Entity, ISoftDeletedEntity
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }
}
