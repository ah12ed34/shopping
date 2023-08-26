using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace shopping.ViewModels
{
    public class LoginVM
    {
        
        [Required,Display(Name ="Username or Email Address")]
        [StringLength(50)]
        [Unicode(false)]
        public string UserName { get; set; }
        [Required,DataType(DataType.Password)
            ,Display(Name ="كلمة المرور")]
        public string Password { get; set; }
        [Display(Name ="تذكرني")]
        public bool RememberMe { get; set; } = false;
    }
}
