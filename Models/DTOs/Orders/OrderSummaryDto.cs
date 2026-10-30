using System;

namespace PSite3.Models.DTOs.Orders;

public class OrderSummaryDto
{
    public int IdOrder { get; set; }
    public int UserId { get; set; }
    public DateTime? OrderDate { get; set; }
}
