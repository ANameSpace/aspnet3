using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PSite3.Models;

public partial class CartItem
{
    public int IdCartItem { get; set; }

    [Required]
    public int CartId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [JsonIgnore]
    public virtual Cart Cart { get; set; } = null!;

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
}
