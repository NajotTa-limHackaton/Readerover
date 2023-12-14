using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readerover.Domain.Entities;

namespace Readerover.Persistence.EntityConfigurations;

public class SubCategoryConfigurations : IEntityTypeConfiguration<SubCategory>
{
    /// <summary>
    /// Sub categoryning baza tarafdagi configuratsiyasi
    /// Category va Subcategory One-To-Many bog'lanadi
    /// </summary>
    /// <param name="builder"></param>
    /// 
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.HasOne(subCategory => subCategory.Category)
            .WithMany(category => category.SubCategories)
            .HasForeignKey(subCategory => subCategory.CategoryId);

        builder.HasQueryFilter(subCategory => !subCategory.IsDeleted);
    }
}
