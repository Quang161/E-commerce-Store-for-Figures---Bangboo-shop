using Bangboo_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class BoorderDetail
    {
        [Key]
        public int IDOrderDetail { get; set; }

        [Required]
        public int IDOrder_DB { get; set; }

        [Required]
        [StringLength(50)]
        public string IDBangboo { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Unit_Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Total_Price { get; set; }

        [ForeignKey("IDOrder_DB")]
        public virtual Boorder Boorder { get; set; }

        [ForeignKey("IDBangboo")]
        public virtual BangbooDb Bangboo { get; set; }
    }
}