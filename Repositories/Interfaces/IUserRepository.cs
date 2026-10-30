using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PSite3.Models;

namespace PSite3.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> UserNameExistsAsync(
        string userName,
        int? excludeId = null,
        CancellationToken cancellationToken = default);
}
