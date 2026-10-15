namespace PSite3.Models.DTOs.Carts;

public static class CartDtoMapper
{
    public static CartDto ToDto(Cart cart)
    {
        return new CartDto
        {
            IdCart = cart.IdCart,
            UserId = cart.UserId
        };
    }
}
