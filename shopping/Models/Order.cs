using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    [Table("orders")]
    public partial class Order
    {
        public Order()
        {
            OrderDaitels = new HashSet<OrderDaitel>();
        }

        [Key]
        public int Id { get; set; }
        [Column("id_member")]
        public int IdMember { get; set; }
        [Column("date_order")]
        public DateTime? DateOrder { get; set; }
        [Column("is_delivery")]
        public bool? IsDelivery { get; set; }
        [Column("delivery_location")]
        [StringLength(500)]
        [Unicode(false),Display(Name ="موقع التوصيل")]
        public string? DeliveryLocation { get; set; }
        [Column("expected_time")]
        public DateTime? ExpectedTime { get; set; }
        [Column("Access_time")]
        public DateTime? AccessTime { get; set; }

        [ForeignKey("IdMember")]
        [InverseProperty("Orders")]
        public virtual Member IdMemberNavigation { get; set; }

        [InverseProperty("IdOrderNavigation")]
        public virtual ICollection<OrderDaitel> OrderDaitels { get; set; }

     
    }
}
