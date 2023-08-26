using shopping.Models;

namespace shopping.ViewModels
{
    public class CartItemVM 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public  decimal Price { get; set; }
        public int Qty { get; set; }
    }
}
