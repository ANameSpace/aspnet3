using AutoMapper;
using PSite3.Models;
using PSite3.Models.DTOs.CartItems;
using PSite3.Models.DTOs.Carts;
using PSite3.Models.DTOs.Categories;
using PSite3.Models.DTOs.OrderItems;
using PSite3.Models.DTOs.Orders;
using PSite3.Models.DTOs.Products;
using PSite3.Models.DTOs.Reservations;
using PSite3.Models.DTOs.Roles;
using PSite3.Models.DTOs.Users;

namespace PSite3.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateReadMaps();
        CreateWriteMaps();
    }

    private void CreateReadMaps()
    {
        CreateMap<Product, ProductSummaryDto>();
        CreateMap<Product, ProductDto>();

        CreateMap<Category, CategorySummaryDto>();
        CreateMap<Category, CategoryDto>();

        CreateMap<Role, RoleSummaryDto>();
        CreateMap<Role, RoleDto>();

        CreateMap<User, UserSummaryDto>();
        CreateMap<User, UserDto>();

        CreateMap<Cart, CartSummaryDto>();
        CreateMap<Cart, CartDto>();

        CreateMap<CartItem, CartItemDto>();

        CreateMap<Order, OrderSummaryDto>();
        CreateMap<Order, OrderDto>();

        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<ProductReservation, ReservationDto>();
    }

    private void CreateWriteMaps()
    {
        CreateMap<CreateProductDto, Product>()
            .ForMember(d => d.IdProduct, o => o.Ignore())
            .ForMember(d => d.CartItems, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForMember(d => d.IdProduct, o => o.Ignore())
            .ForMember(d => d.CartItems, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore());

        CreateMap<PatchProductDto, Product>()
            .ForMember(d => d.IdProduct, o => o.Ignore())
            .ForMember(d => d.CartItems, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.Name, o => o.Condition(src => src.Name != null))
            .ForMember(d => d.Description, o => o.Condition(src => src.Description != null))
            .ForMember(d => d.Price, o => o.Condition(src => src.Price.HasValue))
            .ForMember(d => d.StockQuantity, o => o.Condition(src => src.StockQuantity.HasValue))
            .ForMember(d => d.CategoryId, o => o.Condition(src => src.CategoryId.HasValue))
            .ForMember(d => d.Image, o => o.Condition(src => src.Image != null));

        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.IdUser, o => o.Ignore())
            .ForMember(d => d.Carts, o => o.Ignore())
            .ForMember(d => d.Orders, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.Role, o => o.Ignore());

        CreateMap<UpdateUserDto, User>()
            .ForMember(d => d.IdUser, o => o.Ignore())
            .ForMember(d => d.Carts, o => o.Ignore())
            .ForMember(d => d.Orders, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.Role, o => o.Ignore())
            .ForMember(d => d.Hash, o => o.Condition(src => src.Hash != null));

        CreateMap<PatchUserDto, User>()
            .ForMember(d => d.IdUser, o => o.Ignore())
            .ForMember(d => d.Carts, o => o.Ignore())
            .ForMember(d => d.Orders, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.Role, o => o.Ignore())
            .ForMember(d => d.UserName, o => o.Condition(src => src.UserName != null))
            .ForMember(d => d.Email, o => o.Condition(src => src.Email != null))
            .ForMember(d => d.Hash, o => o.Condition(src => src.Hash != null))
            .ForMember(d => d.RoleId, o => o.Condition(src => src.RoleId.HasValue))
            .ForMember(d => d.IsActive, o => o.Condition(src => src.IsActive.HasValue));

        CreateMap<CreateRoleDto, Role>()
            .ForMember(d => d.IdRole, o => o.Ignore())
            .ForMember(d => d.Users, o => o.Ignore());

        CreateMap<UpdateRoleDto, Role>()
            .ForMember(d => d.IdRole, o => o.Ignore())
            .ForMember(d => d.Users, o => o.Ignore());

        CreateMap<PatchRoleDto, Role>()
            .ForMember(d => d.IdRole, o => o.Ignore())
            .ForMember(d => d.Users, o => o.Ignore())
            .ForMember(d => d.Name, o => o.Condition(src => src.Name != null));

        CreateMap<CreateCategoryDto, Category>()
            .ForMember(d => d.IdCategory, o => o.Ignore())
            .ForMember(d => d.Products, o => o.Ignore());

        CreateMap<UpdateCategoryDto, Category>()
            .ForMember(d => d.IdCategory, o => o.Ignore())
            .ForMember(d => d.Products, o => o.Ignore());

        CreateMap<PatchCategoryDto, Category>()
            .ForMember(d => d.IdCategory, o => o.Ignore())
            .ForMember(d => d.Products, o => o.Ignore())
            .ForMember(d => d.Name, o => o.Condition(src => src.Name != null));

        CreateMap<CreateCartDto, Cart>()
            .ForMember(d => d.IdCart, o => o.Ignore())
            .ForMember(d => d.CartItems, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore());

        CreateMap<UpdateCartDto, Cart>()
            .ForMember(d => d.IdCart, o => o.Ignore())
            .ForMember(d => d.CartItems, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore());

        CreateMap<PatchCartDto, Cart>()
            .ForMember(d => d.IdCart, o => o.Ignore())
            .ForMember(d => d.CartItems, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Condition(src => src.UserId.HasValue));

        CreateMap<CreateCartItemDto, CartItem>()
            .ForMember(d => d.IdCartItem, o => o.Ignore())
            .ForMember(d => d.Cart, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore());

        CreateMap<UpdateCartItemDto, CartItem>()
            .ForMember(d => d.IdCartItem, o => o.Ignore())
            .ForMember(d => d.Cart, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore());

        CreateMap<PatchCartItemDto, CartItem>()
            .ForMember(d => d.IdCartItem, o => o.Ignore())
            .ForMember(d => d.Cart, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore())
            .ForMember(d => d.CartId, o => o.Condition(src => src.CartId.HasValue))
            .ForMember(d => d.ProductId, o => o.Condition(src => src.ProductId.HasValue))
            .ForMember(d => d.Quantity, o => o.Condition(src => src.Quantity.HasValue));

        CreateMap<CreateOrderDto, Order>()
            .ForMember(d => d.IdOrder, o => o.Ignore())
            .ForMember(d => d.OrderDate, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore());

        CreateMap<UpdateOrderDto, Order>()
            .ForMember(d => d.IdOrder, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore());

        CreateMap<PatchOrderDto, Order>()
            .ForMember(d => d.IdOrder, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Reservations, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Condition(src => src.UserId.HasValue))
            .ForMember(d => d.OrderDate, o => o.Condition(src => src.OrderDate.HasValue));

        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(d => d.IdCartItem, o => o.Ignore())
            .ForMember(d => d.Order, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore());

        CreateMap<UpdateOrderItemDto, OrderItem>()
            .ForMember(d => d.IdCartItem, o => o.Ignore())
            .ForMember(d => d.Order, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore());

        CreateMap<PatchOrderItemDto, OrderItem>()
            .ForMember(d => d.IdCartItem, o => o.Ignore())
            .ForMember(d => d.Order, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore())
            .ForMember(d => d.OrderId, o => o.Condition(src => src.OrderId.HasValue))
            .ForMember(d => d.ProductId, o => o.Condition(src => src.ProductId.HasValue))
            .ForMember(d => d.Quantity, o => o.Condition(src => src.Quantity.HasValue))
            .ForMember(d => d.Price, o => o.Condition(src => src.Price.HasValue));

        CreateMap<CreateReservationDto, ProductReservation>()
            .ForMember(d => d.IdReservation, o => o.Ignore())
            .ForMember(d => d.ReservedAt, o => o.Ignore())
            .ForMember(d => d.ClosedAt, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.Order, o => o.Ignore());
    }
}
