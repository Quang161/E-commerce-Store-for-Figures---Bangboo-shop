using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bangboo_E_Commerce.Entities;

public partial class CartBoo
{
    public int IdCartBoo { get; set; }

    public string IdPhatheon { get; set; } = null!;

    [Column("IDBangboo_DB")]
    public string IdBangbooDb { get; set; }

    public int Quantity { get; set; }

    public int? IdOrder { get; set; }

    public virtual Boorder? IdOrderNavigation { get; set; }

    public virtual PhaethonUser IdPhatheonNavigation { get; set; } = null!;

    public virtual BangbooDb IdbangbooDbNavigation { get; set; } = null!;
}
