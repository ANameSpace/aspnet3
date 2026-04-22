using PSite3.Models.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class Category
{
    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int IdCategory { get; set; }

    [Required(ErrorMessage = "Необходимо указать название!")]
    [Name]
    [OnlyRussian]
    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
