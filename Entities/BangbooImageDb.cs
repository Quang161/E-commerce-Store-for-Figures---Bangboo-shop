using System;
using System.Collections.Generic;

namespace Bangboo_E_Commerce.Entities;

public partial class BangbooImageDb
{
    public string IDImage { get; set; } = null!;

    public string? IDImage_Bangboo { get; set; }

    public string Url { get; set; } = null!;

    public virtual BangbooDb? IdimageBangbooNavigation { get; set; }
}
