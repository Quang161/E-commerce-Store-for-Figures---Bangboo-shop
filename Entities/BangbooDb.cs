using System;
using System.Collections.Generic;
using YourNamespace.Models;

namespace Bangboo_E_Commerce.Entities;

public partial class BangbooDb
{
    public string Idbangboo { get; set; } = null!;

    public string NameBangboo { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Rank { get; set; }

    public int Quantity { get; set; }

    public decimal? Price { get; set; }

    public string? Faction { get; set; }

    public string? Attribute { get; set; }

    public virtual ICollection<BangbooImageDb> BangbooImageDbs { get; set; } = new List<BangbooImageDb>();

    public virtual ICollection<CartBoo> CartBoos { get; set; } = new List<CartBoo>();

    public virtual ICollection<BoorderDetail> BoorderDetails { get; set; } = new List<BoorderDetail>();
}
