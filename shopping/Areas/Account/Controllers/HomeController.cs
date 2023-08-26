using Microsoft.AspNetCore.Mvc;

namespace shopping.Areas.Account.Controllers
{
    [Area(nameof(Account))]
    public class HomeController : Controller
    {
        HomeController()
        {
            Home();
        }
        public IActionResult Home()
        {
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index),"home" ,new {Area=""}) ;
        }
    }
}
