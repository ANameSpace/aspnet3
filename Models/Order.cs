using System;
using System.Collections.Generic;

namespace PSite3.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
