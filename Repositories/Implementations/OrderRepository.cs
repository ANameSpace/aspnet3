using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ShopContext context) : base(context)
    {
    }

    public Task<Order?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.IdOrder == id, cancellationToken);

    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsSplitQuery();

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(o => o.IdOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default)
        => await Context.OrderItems.AnyAsync(oi => oi.OrderId == id, cancellationToken)
           || await Context.ProductReservations.AnyAsync(r => r.OrderId == id, cancellationToken);
}
