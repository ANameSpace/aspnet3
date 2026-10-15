using System;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Orders;

public class CreateOrderDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int UserId { get; set; }
}
