using System;
using System.Collections.Generic;

namespace Bangboo_E_Commerce.Entities;

public partial class PhaethonUser
{
    public string IdPhaethon { get; set; } = null!;

    public string FullNamePhatheon { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Boorder> Boorders { get; set; } = new List<Boorder>();

    public virtual ICollection<CartBoo> CartBoos { get; set; } = new List<CartBoo>();
}
