using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using shopping.data.Facade;

namespace shopping.Models
{
    
    [Table("prodects")]
    public partial class Prodect
    {
       
        public Prodect()
        {
            CommentProducts = new HashSet<CommentProduct>();
            OrderDaitels = new HashSet<OrderDaitel>();
            
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_mer")]
        public int IdMer { get; set; }
        [Column("name")]
        [StringLength(50)]
        [Unicode(false),Display(Name ="اسم المونتج")]
        public string Name { get; set; } = null!;
        [Column("home_img")]
        [StringLength(50)]
        [Unicode(false), Display(Name = "الصوره الرائيسية")]
        public string? HomeImg { get; set; }
        [Column("price", TypeName = "money"),Display(Name ="السعر")]
        public decimal Price { get; set; }
        [Column("description")]
        [Unicode(false),Display(Name = "وصف المنتج")]
        public string Description { get; set; } = null!;
        [Column("video_url")]
        [StringLength(250)]
        [Unicode(false), Display(Name = "رابط فيديو")]
        public string? VideoUrl { get; set; }
        [Column("is_Publishing")]
        public bool? IsPublishing { get; set; } 
        [Column("quantity"), Display(Name = "الكمية")]
        public int Quantity { get; set; }
        [Column("img1"), Display(Name = "الصورة 1")]
        [StringLength(100)]
        public string? Img1 { get; set; } = null!;
        [Column("img2"), Display(Name = "الصورة 2")]
        [StringLength(100)]
        public string? Img2 { get; set; } = null!;
        [Column("img3"), Display(Name = "الصورة 3")]
        [StringLength(100)]
        public string? Img3 { get; set; } = null!;
        [Column("img4")]
        [StringLength(100), Display(Name = "الصورة 4")]
        public string? Img4 { get; set; }
        [Column("img5")]
        [StringLength(100), Display(Name = "الصورة 5")]
        public string? Img5 { get; set; }
        [Column("img6")]
        [StringLength(100), Display(Name = "الصورة 6")]
        public string? Img6 { get; set; }
        [Column("img7")]
        [StringLength(100), Display(Name = "الصورة 7")]
        public string? Img7 { get; set; }
        [Column("img8"), Display(Name = "الصورة 8")]
        [StringLength(100)]
        public string? Img8 { get; set; }
        [Column("img9")]
        [StringLength(100), Display(Name = "الصورة 9")]
        public string? Img9 { get; set; }
        [Column("img10")]
        [StringLength(100), Display(Name = "الصورة 10")]
        public string? Img10 { get; set; }

        [ForeignKey("IdMer")]
        [InverseProperty("Prodects")]
        public virtual Merchant? IdMerNavigation { get; set; } = null!;
        [InverseProperty("IdProNavigation")]
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        [InverseProperty("IdProNavigation")]
        public virtual ICollection<OrderDaitel> OrderDaitels { get; set; }
        
    }
}
