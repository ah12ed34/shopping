using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    [Table("Comment_Mer")]
    public partial class CommentMer
    {
        [Key]
        [Column("Id_mer")]
        public int IdMer { get; set; }
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
        [InverseProperty("CommentMers")]
        public virtual Member IdMemberNavigation { get; set; } = null!;
        [ForeignKey("IdMer")]
        [InverseProperty("CommentMers")]
        public virtual Merchant IdMerNavigation { get; set; } = null!;
    }
}
