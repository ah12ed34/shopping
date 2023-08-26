using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopping.Models;
using shopping.ViewModels;
using System.Text;
//using Microsoft.AspNetCore.Http;

namespace shopping.data.Facade
{
    public class FCartItem
    {
        const string CartS = "Cart";
        HttpContext Http;
        
        public FCartItem(IHttpContextAccessor httpContext)
        {
            Http = httpContext.HttpContext;
            
        }
        
        public bool AddToCart(int productId, string productName, decimal productPrice, int Qty)
        {
            var product = new CartItemVM { ProductId = productId, ProductName = productName, Price = productPrice, Qty = Qty };
            if (product != null)
            {
              
                // Get the current cart from the session, or create a new one if it doesn't exist
                List<CartItemVM> cart;
                if (Http.Session.TryGetValue(CartS, out byte[] cartBytes))
                {
                    cart = JsonConvert.DeserializeObject<List<CartItemVM>>(Encoding.UTF8.GetString(cartBytes));
                }
                else
                {
                    cart = new List<CartItemVM>();
                }

                // Add the product to the cart
                cart.Add(product);

                // Save the updated cart back to the session
                Http.Session.Set(CartS, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cart)));
            }

            return true;
            
        }

        public bool RemoveFromCart(int productId)
        {
            // Get the current cart from the session
            List<CartItemVM> cart;
            if (Http.Session.TryGetValue(CartS, out byte[] cartBytes))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemVM>>(Encoding.UTF8.GetString(cartBytes));
            }
            else
            {
                // If the cart doesn't exist, redirect to the cart page
                return false;
            }

            // Find the index of the product to remove
            var index = cart.FindIndex(p => p.ProductId == productId);

            if (index != -1)
            {
                // Remove the product from the cart
                cart.RemoveAt(index);
                // Save the updated cart back to the session
                Http.Session.Set(CartS, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cart)));
            }

            return true;
        }
        public bool updataCart(CartItemVM cartV)
        {
            List<CartItemVM> cart;
            if(Http.Session.TryGetValue(CartS, out byte[] CartBytes))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemVM>>(Encoding.UTF8.GetString(CartBytes));
            }
            else { 
             cart = new List<CartItemVM>();
            }
            int Index = cart.FindIndex(e=>e.ProductId == cartV.ProductId);
            if (Index != -1)
            {
                cart.RemoveAt(Index);
                cart.Insert(Index,cartV);
                Http.Session.Set(CartS, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cart)));
                return true;
            }
            return false;

        }
        public bool CheckCart(int productId)
        {
            // Get the current cart from the session
            List<CartItemVM>? cart;
            if (Http.Session.TryGetValue(CartS, out byte[]? cartBytes))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemVM>>(Encoding.UTF8.GetString(cartBytes));
            }
            else
            {
                // If the cart doesn't exist, redirect to the cart page
                return false;
            }

            // Find the index of the product to remove
            return  cart.Any(e=>e.ProductId==productId);

        }

        public List<CartItemVM> Cart()
        {
            // Get the current cart from the session
            List<CartItemVM> cart;
            if (Http.Session.TryGetValue(CartS, out byte[] cartBytes))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemVM>>(Encoding.UTF8.GetString(cartBytes));
            }
            else
            {
                // If the cart doesn't exist, create a new one
                cart = new List<CartItemVM>();
            }

            //// Calculate the total number of products in the cart
            //var itemCount = cart.Sum(p => p.Quantity);

            //// Pass the cart and item count to the view
            //ViewData["Cart"] = cart;
            //ViewData["ItemCount"] = itemCount;

            return cart;
        }
        public decimal CartCount()
        {
            // Get the current cart from the session
            List<CartItemVM> cart = new List<CartItemVM>();
            if (Http.Session.TryGetValue(CartS, out byte[] cartBytes))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemVM>>(Encoding.UTF8.GetString(cartBytes));
            }
            //else
            //{
            //    // If the cart doesn't exist, create a new one
            //    cart = new List<CartItemVM>();
            //}
            return cart.Count > 0 ? cart.Count : 0;
        }
        public void clear()
        {

            Http.Session.Remove(CartS);
        }
        
    }
}
