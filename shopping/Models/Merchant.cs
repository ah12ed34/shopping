using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    public partial class Merchant
    {
        public Merchant()
        {
            CommentMers = new HashSet<CommentMer>();
            Prodects = new HashSet<Prodect>();
        }

        [Key]
        public int Id { get; set; }
        [Column("Trade_Name")]
        [StringLength(30)]
        [Unicode(false)]
        public string? TradeName { get; set; }
        [Column("is_actvity")]
        public bool? IsActvity { get; set; }
        [Column("locaction")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Locaction { get; set; }
        [Column("tax")]
        public double? Tax { get; set; }
        [Column("earning")]
        public double? Earning { get; set; }

        [ForeignKey("Id")]
        [InverseProperty("Merchant")]
        public virtual Member IdNavigation { get; set; } = null!;
        [InverseProperty("IdMerNavigation")]
        public virtual ICollection<CommentMer> CommentMers { get; set; }
        [InverseProperty("IdMerNavigation")]
        public virtual ICollection<Prodect> Prodects { get; set; }
    }
}
