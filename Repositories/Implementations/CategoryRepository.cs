using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ShopContext context) : base(context)
    {
    }

    public Task<Category?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.IdCategory == id, cancellationToken);

    public async Task<(IReadOnlyList<Category> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(c => c.Products);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(c => c.IdCategory)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default)
        => Context.Products.AnyAsync(p => p.CategoryId == id, cancellationToken);
}
