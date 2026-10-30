using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ShopContext context) : base(context)
    {
    }

    public Task<Cart?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.IdCart == id, cancellationToken);

    public async Task<(IReadOnlyList<Cart> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .AsSplitQuery();

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(c => c.IdCart)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default)
        => Context.CartItems.AnyAsync(ci => ci.CartId == id, cancellationToken);
}
