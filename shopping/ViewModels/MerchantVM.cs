using DataAnnotationsExtensions;
using shopping.Models;
using System.ComponentModel.DataAnnotations;

namespace shopping.ViewModels
{
    public class MerchantVM: Merchant
    {
       
        
      //public Member? r = new RegisterVM();
       [Display(Name ="الاسم التجاري")]
       [Required]
      public new string? TradeName { get { return base.TradeName; } set { base.TradeName = value; } }
        [Display(Name ="موقع المتجر")]
        [Required,MinLength(5)]
        public new string? Locaction { get { return base.Locaction; } set { base.Locaction = value; } }
        [Required,Display(Name ="الضريبة%"), Min(0), Max(100)]
        public new double? Tax { get { return base.Tax; } set { base.Tax = value ; } }


    }
}
