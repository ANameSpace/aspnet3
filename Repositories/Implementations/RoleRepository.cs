using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ShopContext context) : base(context)
    {
    }

    public Task<Role?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.IdRole == id, cancellationToken);

    public async Task<(IReadOnlyList<Role> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(r => r.Users);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(r => r.IdRole)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> HasDependenciesAsync(int id, CancellationToken cancellationToken = default)
        => Context.Users.AnyAsync(u => u.RoleId == id, cancellationToken);

    public Task<bool> NameExistsAsync(
        string name,
        int? excludeId = null,
        CancellationToken cancellationToken = default)
        => Query().AnyAsync(
            r => r.Name == name && (!excludeId.HasValue || r.IdRole != excludeId.Value),
            cancellationToken);
}
