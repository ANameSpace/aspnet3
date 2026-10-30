using System.Collections.Generic;
using PSite3.Models.DTOs.CartItems;
using PSite3.Models.DTOs.Users;

namespace PSite3.Models.DTOs.Carts;

public class CartDto
{
    public int IdCart { get; set; }
    public int UserId { get; set; }
    public UserSummaryDto? User { get; set; }
    public List<CartItemDto> CartItems { get; set; } = new();
}
