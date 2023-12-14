namespace Readerover.Api.Common.Dtos;

public class SubCategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public Guid CategoryId { get; set; }
}
