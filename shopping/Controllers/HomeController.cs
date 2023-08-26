using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping.Models;
using shopping.ViewModels;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using shopping.data.Repositories;
using shopping.data.Facade;

namespace shopping.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly shoppingContext _Contect;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Prodect> _repositoryProd;
        private readonly FCartItem  fCard;
        private readonly FProduct  fProd;
        
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public HomeController(ILogger<HomeController> logger, shoppingContext shopping
            , IHttpContextAccessor httpContextAccessor, IRepository<Prodect> repository,
            FCartItem fCartItem, FProduct fProduct
            )
        {

            _Contect = shopping;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _repositoryProd = repository;
            fCard = fCartItem;
            fProd = fProduct;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 1)
        {
           
            List<Prodect> prodects = new List<Prodect>();
            prodects = await _Contect.Prodects
                .Include(e=>e.IdMerNavigation).Include(connectionString => connectionString.CommentProducts)
                .Where(e=>e.IsPublishing == true&& e.Quantity>=0).Skip((page-1)*pageSize).Take(pageSize).ToListAsync() ;
            ViewBag.TotalItems = _Contect.Prodects.Where(e => e.IsPublishing == true).Count();
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            PagerModel pager = new PagerModel(_Contect.Prodects.Where(e => e.IsPublishing == true).Count(), page, pageSize);
            ViewBag.Page = pager;
            ViewData["Card"] = fCard;

            return View(prodects);
        }
      
        //[Authorize(Roles = "CONTACT")]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult detailProdect(int PID)
        {
            Prodect prod = new Prodect();
            try
            {
                if (PID == 0 )
                {
                    return RedirectToAction("Index");
                }
                prod = _Contect.Prodects.Include(e => e.CommentProducts).ThenInclude(e => e.IdMemberNavigation)
                   .Include(e => e.IdMerNavigation).ThenInclude(e => e.IdNavigation)
                   .Where(e => e.Id == PID).FirstOrDefault();
                if (prod != null)
                {

                    if (prod.IsPublishing != true)
                    {
                        TempData["Msg"] = "لم يتم بعد الموافقة على المنتج";
                        TempData["Code"] = 2;
                        return RedirectToAction("Index");
                    }
                    if (prod.Quantity <= 0)
                    {
                        TempData["Msg"] = "نفذة الكمية";
                        TempData["Code"] = 2;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Msg"] = "لم يتم العثور على المنتج";
                    TempData["Code"] = 2;
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            

            return View(prod);
        }
        public IActionResult delProd()
        {
            return View();
        }
        public IActionResult editCard(CartItemVM item)
        {
            bool b;
            if (item != null)
            {
                Prodect? prodect = _Contect.Prodects.Include(e => e.IdMerNavigation).ThenInclude(e => e.IdNavigation).Where(e => e.Id == item.ProductId).FirstOrDefault();
                if (prodect != null)
                {
                    item.ProductName = prodect.Name;
                    item.Price = fProd.totle_price(prodect.Price, prodect.IdMerNavigation.Tax, prodect.IdMerNavigation.Earning);

                    if (prodect.Quantity >= item.Qty)
                    {
                        b = fCard.updataCart(item);
                    }
                    else
                    { item.Qty = prodect.Quantity;
                        b = fCard.updataCart(item);
                        TempData["Msg"] = $"لم يتم اضافة الكمية المطلوبة بسب عدم توفره تم اضافة{prodect.Quantity}\t\t\n";
                    }
                    if (b == false)
                        TempData["Msg"] = "لم يتم اضافة منتج للسله";
                    else TempData["Msg"] += "Done add";
                }
                else
                {
                    TempData["Msg"] = "لم يتم العثور على المنتج";
                }
            }
            return RedirectToAction(nameof(Cart));
        }
        [Authorize]
        public IActionResult Checkout()
        {
            if (fCard.CartCount() <= 0)
            {
                TempData["Msg"] = "لايوجد منتجات في السلة";
                TempData["Code"] = 2;
                return RedirectToAction(nameof(Index), "Home");
            }
            int id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Order order = new Order();
            order.IdMemberNavigation = _Contect.Members.Where(e=>e.Id ==id).FirstOrDefault();
            return View(order);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            try
            {
                if (fCard.CartCount() <= 0)
                {
                    TempData["Msg"] = "لايوجد منتجات في السلة";
                    TempData["Code"] = 2;
                    return RedirectToAction(nameof(Index), "Home");
                }
                int id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                order.IdMemberNavigation = _Contect.Members.Where(e => e.Id == id).FirstOrDefault();
                order.ExpectedTime = DateTime.UtcNow.AddHours(2);
                List<Prodect> p = new List<Prodect>();
                int prodInt = 0;
                foreach (var item in fCard.Cart())
                {
                    
                    p.Add(_Contect.Prodects.Include(e => e.IdMerNavigation).Where(e => e.Id == item.ProductId).FirstOrDefault());
                    if (p[prodInt].Quantity > item.Qty)
                        p[prodInt].Quantity -= item.Qty;
                    else
                        p[prodInt].Quantity -= item.Qty = p[prodInt].Quantity;
                    order.OrderDaitels.Add(new OrderDaitel
                    {
                        IdPro = item.ProductId
                        ,
                        Price = p[prodInt].Price,
                        Tax = p[prodInt].IdMerNavigation.Tax,
                        Earning = p[prodInt].IdMerNavigation.Earning
                        ,
                        Quantity = item.Qty
                    });
                    prodInt++;
                }
                _Contect.Orders.Add(order);
                if (_Contect.SaveChanges() > 0)
                {
                    if (p != null)
                    {
                        foreach (var item in p)
                            _repositoryProd.Update(item);
                    }
                    fCard.clear();
                }

            }catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                TempData["Code"] = 3; 
            }


            return RedirectToAction(nameof(Index),"Home");
        }
        public IActionResult review(IFormCollection IForm)
        {
           
            
            try
            {
                CommentProduct commentProduct = new CommentProduct() { IdMember = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value) 
             , IdPro = Convert.ToInt32( IForm["PID"]) , Star = Convert.ToInt32(IForm["rating"]) 
             , Comment = IForm["Comment"]
            };
                _Contect.CommentProducts.Add(commentProduct);
                _Contect.SaveChanges();

                return RedirectToAction("Index",new {PID = Convert.ToInt32(IForm["PID"])});
            }catch (Exception ex)
            {
                
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public  async Task<IActionResult> Search(string ProductName = " ", int page=1,int pageSize=10)
        {
            List<Prodect> prodects = new List<Prodect>();
            prodects = await _repositoryProd.Get(page, pageSize).Where(e => (e.Name.Contains(ProductName) || e.Description.Contains(ProductName))&&e.IsPublishing==true)
                .Include(e => e.IdMerNavigation).Include(e => e.CommentProducts)
                .ToListAsync();
            ViewBag.TotalItems = _Contect.Prodects.Where(e=>(e.Name.Contains(ProductName) ||e.Description.Contains(ProductName))&&e.IsPublishing==true).Count();
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.SName = ProductName;

            return View(prodects);
        }
        [HttpPost]
        
        public IActionResult addCart(IFormCollection Iform)
        {
            bool b;
            if(Iform != null)
            {
                int Id = Convert.ToInt32(Iform["PID"]);
                int Qty = Convert.ToInt32(Iform["qty"]);
               Prodect? prodect = _Contect.Prodects.Include(e=>e.IdMerNavigation).ThenInclude(e=>e.IdNavigation).Where(e=>e.Id==Id).FirstOrDefault();
                if(prodect != null)
                {
                    
                    if (fCard.CheckCart(prodect.Id))
                    {
                        TempData["Code"] = 2;
                        TempData["Msg"] = "المنتج مضاف مسبقاً";
                        return RedirectToAction(nameof(detailProdect), "home", new { PID = Iform["PID"] });
                    }
                    if(prodect.Quantity >= Qty)
                    {
                      b =  fCard.AddToCart(prodect.Id,prodect.Name,fProd.totle_price(prodect.Price,prodect.IdMerNavigation.Tax ,prodect.IdMerNavigation.Earning),Qty);
                    }
                    else
                    {
                      b=  fCard.AddToCart(prodect.Id, prodect.Name,fProd.totle_price(prodect.Price,prodect.IdMerNavigation.Tax, prodect.IdMerNavigation.Earning), prodect.Quantity);
                        TempData["Msg"] = $"لم يتم اضافة الكمية المطلوبة بسب عدم توفره تم اضافة{prodect.Quantity}\t\t\n";
                    }
                    if(b == false)
                    TempData["Msg"] = "لم يتم اضافة منتج للسله";
                    else TempData["Msg"] += "Done add";
                }
                else
                {
                    TempData["Msg"] = "لم يتم العثور على المنتج";
                }

            }
            return RedirectToAction(nameof(detailProdect),"home",new {PID = Iform["PID"] });
        }


        public IActionResult Cart()
        {
            if(fCard.CartCount()<=0)
            {
                TempData["Msg"] = "لايوجد منتجات في السلة";
                TempData["Code"] = 2 ;
                return RedirectToAction(nameof(Index),"Home");
            }
            return View(fCard.Cart());
        }
        public IActionResult removeCart(int PID)
        {
            fCard.RemoveFromCart(PID);
            return RedirectToAction(nameof(Cart),"Home");
        }
        [Authorize]
        public IActionResult View_invoice(int OID)
        {
            int UID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Order? o = _Contect.Orders.Include(e=>e.OrderDaitels).ThenInclude(e=>e.IdProNavigation).Include(e=>e.IdMemberNavigation)
                .Where(e=>e.IdMemberNavigation.Id==UID).Where(e=>e.Id==OID).FirstOrDefault();
            return View(o);
        }
        [Authorize]
        public IActionResult Invoice()
        {
            int? UID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Order> o = _Contect.Orders.Include(e=>e.OrderDaitels).Include(e=>e.IdMemberNavigation)
                .Where(e=>e.IdMemberNavigation.Id == UID ).ToList();
            return View(o);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("/error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}