using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    public partial class Role
    {
        public Role()
        {
            IdMs = new HashSet<Member>();
        }

        [Key]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(60)]
        [Unicode(false)]
        public string? Name { get; set; }

        [ForeignKey("IdR")]
        [InverseProperty("IdRs")]
        public virtual ICollection<Member> IdMs { get; set; }
    }
}
