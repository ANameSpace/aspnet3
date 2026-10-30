using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ShopContext context) : base(context)
    {
    }

    public Task<User?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(u => u.Role)
            .Include(u => u.Carts)
            .Include(u => u.Orders)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.IdUser == id, cancellationToken);

    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(u => u.Role)
            .Include(u => u.Carts)
            .Include(u => u.Orders)
            .AsSplitQuery();

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(u => u.IdUser)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default)
        => await Context.Carts.AnyAsync(c => c.UserId == id, cancellationToken)
           || await Context.Orders.AnyAsync(o => o.UserId == id, cancellationToken)
           || await Context.ProductReservations.AnyAsync(r => r.UserId == id, cancellationToken);

    public Task<bool> UserNameExistsAsync(
        string userName,
        int? excludeId = null,
        CancellationToken cancellationToken = default)
        => Query().AnyAsync(
            u => u.UserName == userName && (!excludeId.HasValue || u.IdUser != excludeId.Value),
            cancellationToken);
}
