using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YourNamespace.Models;

namespace Bangboo_E_Commerce.Entities;

public partial class Boorder
{
    public int Idorder { get; set; }

    public string IdPhaethon { get; set; } = null!;

    public string PaymentMethod { get; set; }

    public string TransactionId { get; set; }

    public string SellerInfo { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public virtual PhaethonUser IdPhatheonNavigation { get; set; } = null!;

    public virtual ICollection<CartBoo> CartBoos { get; set; } = new List<CartBoo>();

    public virtual ICollection<BoorderDetail> BoorderDetails { get; set; } = new List<BoorderDetail>();
}
