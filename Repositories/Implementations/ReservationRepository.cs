using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class ReservationRepository : Repository<ProductReservation>, IReservationRepository
{
    public ReservationRepository(ShopContext context) : base(context)
    {
    }

    public Task<ProductReservation?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => Query()
            .AsNoTracking()
            .Include(r => r.Product)
            .Include(r => r.User)
            .Include(r => r.Order)
            .FirstOrDefaultAsync(r => r.IdReservation == id, cancellationToken);

    public Task<ProductReservation?> GetActiveWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default)
        => Query()
            .Include(r => r.Product)
            .Include(r => r.User)
            .Include(r => r.Order)
            .FirstOrDefaultAsync(
                r => r.IdReservation == id && r.Status == ReservationStatus.Active,
                cancellationToken);

    public async Task<(IReadOnlyList<ProductReservation> Items, int TotalCount)> GetPagedWithDetailsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Query()
            .AsNoTracking()
            .Include(r => r.Product)
            .Include(r => r.User)
            .Include(r => r.Order);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(r => r.IdReservation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
