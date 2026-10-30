using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PSite3.Models;

namespace PSite3.Repositories.Interfaces;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<OrderItem?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<OrderItem> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> PairExistsAsync(
        int orderId,
        int productId,
        int? excludeId = null,
        CancellationToken cancellationToken = default);
}
