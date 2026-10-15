using System;

namespace PSite3.Models.DTOs.Orders;

public class OrderDto
{
    public int IdOrder { get; set; }
    public int UserId { get; set; }
    public DateTime? OrderDate { get; set; }
}
