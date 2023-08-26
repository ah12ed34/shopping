using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping.Models;
using System.ComponentModel.DataAnnotations;

namespace shopping.Areas.Account.Models
{
    public class Register : Member
    {
        [StringLength(50)]
        [Unicode(false), Display(Name = "اسم المستخدم"), RegularExpression(@"^[a-zA-Z0-9]{5,50}$",ErrorMessage ="يجب ان يكون A-z او ارقام ويكون طول من 5 الى 50")]
        [Remote(action: "CheckUserName", controller: "Validation", ErrorMessage = "اسم المستخدم موجود بالفعل")]
        public new string UserName { get { return base.UserName; } set { base.UserName = value; } }
        [StringLength(100)]
        [Required, Unicode(false), Display(Name = "الايميل"), EmailAddress(ErrorMessage ="بريد الكتروني غير صالح")]
        [Remote(action: "CheckEmail", controller: "Validation", ErrorMessage = "بريد الكتروني موجود بالفعل")]
        public new string Email { get { return base.Email; } set { base.Email = value; } }
    }
}
