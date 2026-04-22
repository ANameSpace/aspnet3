using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class OrderItem
{
    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int IdCartItem { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Необходимо указать кол-во!")]
    [Range(1, int.MaxValue, ErrorMessage = "Кол-во не может быть таким числом!")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Необходимо указать цену!")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Цена не может быть таким числом!")]
    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
