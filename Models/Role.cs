using PSite3.Models.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSite3.Models;

public partial class Role
{
    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Range(0, int.MaxValue, ErrorMessage = "Идентефикатор не может быть таким числом!")]
    public int IdRole { get; set; }

    [Required(ErrorMessage = "Необходимо указать идентефикатор!")]
    [Name]
    [OnlyRussian]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
