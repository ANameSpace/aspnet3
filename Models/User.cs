using PSite3.Models.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class User
{
    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int IdUser { get; set; }

    [Required(ErrorMessage = "Необходимо указать имя пользователя!")]
    [StringLength(64, MinimumLength = 3, ErrorMessage = "Поле от 3 до 64 символоы")]
    [Name]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо указать почту!")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо указать хэш!")]
    public string Hash { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductReservation> Reservations { get; set; } = new List<ProductReservation>();

    public virtual Role Role { get; set; } = null!;
}
