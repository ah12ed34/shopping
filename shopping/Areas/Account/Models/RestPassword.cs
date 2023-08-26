using System.ComponentModel.DataAnnotations;

namespace shopping.Areas.Account.Models
{
    public class RestPassword
    {
        public int Id { get; set; }
        [DataType(DataType.Password)]
        public string PasswordOld { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password),ErrorMessage ="كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set;}
        
    }
}
