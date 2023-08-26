using Microsoft.AspNetCore.Mvc;

namespace shopping.Areas.Account.Controllers
{
    [Area(nameof(Account))]
    [Route("Account/AccessDenied")]
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
