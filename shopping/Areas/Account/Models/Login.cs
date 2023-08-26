using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace shopping.Areas.Account.Models
{
    [Area(nameof(Account))]
    public class Login
    {

        [Required, Display(Name = "اسم المستخدم او البريد الكتروني")]
        [StringLength(50)]
        [Unicode(false)]
        public string UserName { get; set; } = null!;
        [Required,DataType(DataType.Password)
            ,Display(Name ="كلمة المرور")]
        public string Password { get; set; } = null!;
        [Display(Name ="تذكرني")]
        public bool RememberMe { get; set; } = false;
    }
}
