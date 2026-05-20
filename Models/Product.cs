using PSite3.Models.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class Product
{
    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int IdProduct { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Name]
    public string Name { get; set; } = null!;

    [StringLength(64, MinimumLength = 3, ErrorMessage = "Поле от 3 до 64 символоы")]
    [Censorship]
    [Sentence]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Необходимо указать цену!")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Цена не может быть таким числом!")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int CategoryId { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
