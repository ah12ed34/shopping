using Microsoft.AspNetCore.Mvc;
using shopping.Models;

namespace shopping.Areas.DealerDB.Controllers
{
    [Area("DealerDB")]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

    }
}
