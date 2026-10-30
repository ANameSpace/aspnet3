using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ShopContext context) : base(context)
    {
    }

    public Task<Product?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.IdProduct == id, cancellationToken);

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(p => p.Category);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.IdProduct)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default)
        => await Context.CartItems.AnyAsync(ci => ci.ProductId == id, cancellationToken)
           || await Context.OrderItems.AnyAsync(oi => oi.ProductId == id, cancellationToken)
           || await Context.ProductReservations.AnyAsync(r => r.ProductId == id, cancellationToken);
}
