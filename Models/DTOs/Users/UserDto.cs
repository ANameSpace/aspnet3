using System.Collections.Generic;
using PSite3.Models.DTOs.Carts;
using PSite3.Models.DTOs.Orders;
using PSite3.Models.DTOs.Roles;

namespace PSite3.Models.DTOs.Users;

public class UserDto
{
    public int IdUser { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public RoleSummaryDto? Role { get; set; }
    public List<OrderSummaryDto> Orders { get; set; } = new();
    public List<CartSummaryDto> Carts { get; set; } = new();
}
