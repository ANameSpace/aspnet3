namespace PSite3.Models.DTOs.OrderItems;

public static class OrderItemDtoMapper
{
    public static OrderItemDto ToDto(OrderItem orderItem)
    {
        return new OrderItemDto
        {
            IdCartItem = orderItem.IdCartItem,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price
        };
    }
}
