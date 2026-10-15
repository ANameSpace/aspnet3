namespace PSite3.Models.DTOs.Orders;

public static class OrderDtoMapper
{
    public static OrderDto ToDto(Order order)
    {
        return new OrderDto
        {
            IdOrder = order.IdOrder,
            UserId = order.UserId,
            OrderDate = order.OrderDate
        };
    }
}
