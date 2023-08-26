using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    [Table("Comment_product")]
    public partial class CommentProduct
    {
        [Key]
        [Column("Id_Pro")]
        public int IdPro { get; set; }
        [Key]
        [Column("Id_member")]
        public int IdMember { get; set; }
        [Column("star", TypeName = "numeric(1, 0)"),Range(1,5)]
        public decimal? Star { get; set; }
        [Column("comment")]
        [StringLength(5000)]
        [Unicode(false)]
        public string? Comment { get; set; }

        [ForeignKey("IdMember")]
        [InverseProperty("CommentProducts")]
        public virtual Member IdMemberNavigation { get; set; } = null!;
        [ForeignKey("IdPro")]
        [InverseProperty("CommentProducts")]
        public virtual Prodect IdProNavigation { get; set; } = null!;
    }
}
