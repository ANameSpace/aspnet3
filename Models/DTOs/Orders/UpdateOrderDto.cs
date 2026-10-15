using System;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Orders;

public class UpdateOrderDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    public DateTime? OrderDate { get; set; }
}
