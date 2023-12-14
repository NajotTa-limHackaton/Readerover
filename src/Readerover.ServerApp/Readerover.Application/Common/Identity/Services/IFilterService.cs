using Readerover.Application.Common.Models.Filtering;
using Readerover.Domain.Entities;

namespace Readerover.Application.Common.Identity.Services;

public interface IFilterService
{
    ValueTask<FilterDataModel> GetFilterDataModelAsync(CancellationToken cancellationToken = default);

    ValueTask<Book> GetFilteredBooksAsync(FilterModel filterModel, CancellationToken cancellationToken = default);
}
