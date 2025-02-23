using Bangboo_E_Commerce.Entities;

namespace Bangboo_E_Commerce.Models
{
    public class BangbooDetailsViewModel
    {
        public virtual BangbooDb? IdimageBangbooNavigation { get; set; }
        public string IDImage_Bangboo { get; set; }
        public string IDImage { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
        public string Attribute { get; set; }
        public decimal? Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
