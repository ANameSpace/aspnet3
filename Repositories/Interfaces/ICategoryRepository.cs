using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PSite3.Models;

namespace PSite3.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Category> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default);
}
