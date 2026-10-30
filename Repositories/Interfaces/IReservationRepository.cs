using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PSite3.Models;

namespace PSite3.Repositories.Interfaces;

public interface IReservationRepository : IRepository<ProductReservation>
{
    Task<ProductReservation?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<ProductReservation?> GetActiveWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ProductReservation> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
