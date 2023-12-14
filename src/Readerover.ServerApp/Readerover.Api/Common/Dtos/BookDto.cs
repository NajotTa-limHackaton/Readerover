namespace Readerover.Api.Common.Dtos;

public class BookDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public Guid CategoryId { get; set; }

    public Guid SubCategoryId { get; set; }

    public Guid AuthorId { get; set; }

    public string BookUrl { get; set; } = default!;

    public string AudioUrl { get; set; } = default!;
}
