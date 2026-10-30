using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PSite3.Models;

namespace PSite3.Repositories.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Role> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(
        string name,
        int? excludeId = null,
        CancellationToken cancellationToken = default);
}
