using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopping.Models
{
    [Table("members")]
    public partial class Member
    {
        public Member()
        {
            CommentMers = new HashSet<CommentMer>();
            CommentProducts = new HashSet<CommentProduct>();
            Orders = new HashSet<Order>();
            IdRs = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }
        [Column("fname"),Display(Name ="الاسم الاول")]
        [StringLength(20),RegularExpression(@"^([\u0621-\u064A]{2,20})|([a-zA-Z]{2,20})$")]
        [Unicode(false)]
        public string Fname { get; set; } = null!;
        [Column("mname")]
        [StringLength(20), RegularExpression(@"^([\u0621-\u064A]{2,20})|([a-zA-Z]{2,20})$")]
        [Unicode(false), Display(Name = "الاسم الوسط")]
        public string Mname { get; set; } = null!;
        [Column("lname")]
        [StringLength(20), RegularExpression(@"^([\u0621-\u064A\-]{2,20})|([a-zA-Z\-]{2,20})$")]
        [Unicode(false), Display(Name = "اللقب")]
        public string Lname { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false), Display(Name = "اسم المستخدم"),RegularExpression(@"^[a-zA-Z0-9]{5,50}$")]
        public string UserName { get; set; } = null!;
        [StringLength(100)]
        [Required, Unicode(false), Display(Name = "الايميل"),EmailAddress]
        public string Email { get; set; } = null!;
        [Column("password"), Display(Name = "كلمة المرور")]
        [Unicode(false),DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [NotMapped,DataType(DataType.Password),Display(Name ="تاكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
        [Column("profile")]
        [StringLength(150)]
        [Unicode(false)]
        public string? Profile { get; set; }
        [Column(TypeName = "numeric(15, 0)"), Display(Name = "رقم الهاتف")]
        public decimal? Phone { get; set; }
        [Column("city")]
        [StringLength(20), Display(Name = "المدينة")]
        [Unicode(false)]
        public string? City { get; set; }
        [Column("country"), Display(Name = "الدولة")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Country { get; set; }
        [Column("address")]
        [StringLength(30)]
        [Unicode(false), Display(Name = "العنوان")]
        public string? Address { get; set; }
        [Column("type_user")]
        public int? TypeUser { get; set; }
        [Column("birthday", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "تاريخ الميلاد"),DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        [Column("join_date")]
        public DateTime? JoinDate { get; set; }
        [Column("lest_date")]
        public DateTime? LestDate { get; set; }
        [Column("pass_lest_date")]
        public DateTime? PassLestDate { get; set; }

        [InverseProperty("IdNavigation")]
        public virtual Merchant? Merchant { get; set; }
        [InverseProperty("IdMemberNavigation")]
        public virtual ICollection<CommentMer> CommentMers { get; set; }
        [InverseProperty("IdMemberNavigation")]
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }
        [ForeignKey("IdM")]
        [InverseProperty("IdMs")]
        public virtual ICollection<Role> IdRs { get; set; }
    }
}
