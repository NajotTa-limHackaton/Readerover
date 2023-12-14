using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Filtering;
using Readerover.Domain.Entities;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class FilterService(
    ICategoryService categoryService,
    ISubCategoryService subCategoryService) : IFilterService
{
    public ValueTask<FilterDataModel> GetFilterDataModelAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //return new FilterDataModel()
        //{
        //    Categories = categoryService.Get(),B
        //    SubCategories = subCategoryService.Get(),
        //    Authors = 


        //}
    }

    public ValueTask<Book> GetFilteredBooksAsync(FilterModel filterModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
