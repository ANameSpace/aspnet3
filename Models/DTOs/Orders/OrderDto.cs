using System;
using System.Collections.Generic;
using PSite3.Models.DTOs.OrderItems;
using PSite3.Models.DTOs.Users;

namespace PSite3.Models.DTOs.Orders;

public class OrderDto
{
    public int IdOrder { get; set; }
    public int UserId { get; set; }
    public DateTime? OrderDate { get; set; }
    public UserSummaryDto? User { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}
