using shopping.Models;

namespace shopping.ViewModels
{
    public class DashboardVM : LayoutVM
    {
       public DashboardVM()
        {
            name1 = "المستخدمين";
            name2 = "التجار";
        }
      public  IQueryable<Member> members;
      public  IQueryable<Merchant> merchant;
      public  IQueryable<Prodect> prodect;
      public  IQueryable<OrderDaitel> orderDaitels;
      public  IQueryable<Order> orders;
       
    }
}
