using PSite3.Models.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class Order
{
    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int IdOrder { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int UserId { get; set; }

    [NotFutureDate]
    public DateTime? OrderDate { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductReservation> Reservations { get; set; } = new List<ProductReservation>();

    public virtual User User { get; set; } = null!;
}
