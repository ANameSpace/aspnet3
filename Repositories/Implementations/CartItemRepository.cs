using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class CartItemRepository : Repository<CartItem>, ICartItemRepository
{
    public CartItemRepository(ShopContext context) : base(context)
    {
    }

    public Task<CartItem?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.IdCartItem == id, cancellationToken);

    public async Task<(IReadOnlyList<CartItem> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(ci => ci.Product);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(ci => ci.IdCartItem)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> PairExistsAsync(
        int cartId,
        int productId,
        int? excludeId = null,
        CancellationToken cancellationToken = default)
        => Query().AnyAsync(
            ci => ci.CartId == cartId
                  && ci.ProductId == productId
                  && (!excludeId.HasValue || ci.IdCartItem != excludeId.Value),
            cancellationToken);
}
