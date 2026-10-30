using System;
using PSite3.Models.DTOs.Orders;
using PSite3.Models.DTOs.Products;
using PSite3.Models.DTOs.Users;

namespace PSite3.Models.DTOs.Reservations;

public class ReservationDto
{
    public int IdReservation { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int? OrderId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public ReservationStatus Status { get; set; }
    public ProductSummaryDto? Product { get; set; }
    public UserSummaryDto? User { get; set; }
    public OrderSummaryDto? Order { get; set; }
}
