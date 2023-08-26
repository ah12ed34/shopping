using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    [Table("orderDaitels")]
    public partial class OrderDaitel
    {
        [Key]
        [Column("Id_order")]
        public int IdOrder { get; set; }
        [Key]
        [Column("Id_pro")]
        public int IdPro { get; set; }
        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double? Tax { get; set; }
        [Column("earning")]
        public double? Earning { get; set; }

        [ForeignKey("IdOrder")]
        [InverseProperty("OrderDaitels")]
        public virtual Order IdOrderNavigation { get; set; } = null!;
        [ForeignKey("IdPro")]
        [InverseProperty("OrderDaitels")]
        public virtual Prodect IdProNavigation { get; set; } = null!;
    }
}
