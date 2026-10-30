using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ShopContext context) : base(context)
    {
    }

    public Task<OrderItem?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.IdCartItem == id, cancellationToken);

    public async Task<(IReadOnlyList<OrderItem> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(oi => oi.Product);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(oi => oi.IdCartItem)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> PairExistsAsync(
        int orderId,
        int productId,
        int? excludeId = null,
        CancellationToken cancellationToken = default)
        => Query().AnyAsync(
            oi => oi.OrderId == orderId
                  && oi.ProductId == productId
                  && (!excludeId.HasValue || oi.IdCartItem != excludeId.Value),
            cancellationToken);
}
