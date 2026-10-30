using System;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class ProductReservation
{
    public int IdReservation { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентификатор товара!")]
    [Range(1, int.MaxValue, ErrorMessage = "Идентификатор товара должен быть положительным!")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентификатор пользователя!")]
    [Range(1, int.MaxValue, ErrorMessage = "Идентификатор пользователя должен быть положительным!")]
    public int UserId { get; set; }

    public int? OrderId { get; set; }

    [Required(ErrorMessage = "Необходимо указать количество!")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть положительным!")]
    public int Quantity { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Active;

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
