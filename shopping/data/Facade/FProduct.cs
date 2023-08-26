namespace shopping.data.Facade
{
    public class FProduct
    {
        
        public decimal totle_price(decimal price,double? tax,double? l)
        {
            return Math.Round((Math.Round(Convert.ToDecimal(tax),2) *price )+(Math.Round(Convert.ToDecimal(l),2) *price)+price,2);
        }
    }
}
