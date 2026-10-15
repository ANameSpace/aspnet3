namespace PSite3.Models.DTOs.CartItems;

public static class CartItemDtoMapper
{
    public static CartItemDto ToDto(CartItem cartItem)
    {
        return new CartItemDto
        {
            IdCartItem = cartItem.IdCartItem,
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        };
    }
}
