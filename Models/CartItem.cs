using Newtonsoft.Json;

namespace Bangboo_E_Commerce.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal Total => (Price ?? 0) * Quantity;
    }
}
