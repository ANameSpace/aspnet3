using System;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.DTOs.Orders;

public class PatchOrderDto
{
    [Range(0, int.MaxValue)]
    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }
}
