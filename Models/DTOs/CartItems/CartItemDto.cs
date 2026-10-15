namespace PSite3.Models.DTOs.CartItems;

public class CartItemDto
{
    public int IdCartItem { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
