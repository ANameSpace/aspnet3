using System.Threading;
using System.Threading.Tasks;
using PSite3.Models.DTOs.Reservations;
using PSite3.Services.Common;

namespace PSite3.Services.Interfaces;

public interface IReservationService
{
    Task<ServiceResult<PagedResult<ReservationDto>>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<ReservationDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<ReservationDto>> CreateAsync(
        CreateReservationDto dto,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<ReservationDto>> CancelAsync(
        int id,
        CancellationToken cancellationToken = default);
}
