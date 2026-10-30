using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSite3.Models.DTOs.Reservations;
using PSite3.Services.Interfaces;

namespace PSite3.Controllers.Api;

[Route("api/reservations")]
[ApiController]
public class ReservationsApiController : ApiControllerBase
{
    private readonly IReservationService _reservations;

    public ReservationsApiController(IReservationService reservations)
    {
        _reservations = reservations;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => FromPagedResult(await _reservations.GetPagedAsync(page, pageSize, cancellationToken));

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetReservation(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _reservations.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> CreateReservation(
        CreateReservationDto dto,
        CancellationToken cancellationToken = default)
        => CreatedFromResult(
            await _reservations.CreateAsync(dto, cancellationToken),
            nameof(GetReservation),
            reservation => reservation.IdReservation);

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ReservationDto>> CancelReservation(
        int id,
        CancellationToken cancellationToken = default)
        => FromResult(await _reservations.CancelAsync(id, cancellationToken));
}
