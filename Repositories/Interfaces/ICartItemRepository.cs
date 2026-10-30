using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PSite3.Models;

namespace PSite3.Repositories.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<CartItem?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<CartItem> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> PairExistsAsync(
        int cartId,
        int productId,
        int? excludeId = null,
        CancellationToken cancellationToken = default);
}
