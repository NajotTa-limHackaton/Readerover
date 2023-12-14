using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readerover.Domain.Entities;

namespace Readerover.Persistence.EntityConfigurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasIndex(book => book.Name).IsUnique();

        builder.HasQueryFilter(book => !book.IsDeleted);

        builder
            .HasOne(book => book.Author)
            .WithMany(author => author.Books)
            .HasForeignKey(book => book.AuthorId);

        builder
            .HasOne(book => book.Category)
            .WithMany(category => category.Books)
            .HasForeignKey(book => book.CategoryId);

        builder.HasOne(book => book.SubCategory)
            .WithMany(subCategory => subCategory.Books)
            .HasForeignKey(book => book.SubCategoryId);


    }
}
