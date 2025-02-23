using Bangboo_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;

namespace Bangboo_E_Commerce.Models
{
    public class BangbooImage_Code
    {
        [Key]
        public string IDImage { get; set; }
        public string IDImage_Bangboo { get; set; }
        public string URL { get; set; }
        public string NameBangboo { get; set; }
        public string Description { get; set; }
        public string? Rank { get; set; }
        public decimal? Price { get; set; }
        public string? Faction { get; set; }
        public string? Attribute { get; set; }
        public string? Idbangboo { get; set; }
        public virtual BangbooDb? IdimageBangbooNavigation { get; set; }
    }
}
