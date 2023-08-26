using Microsoft.AspNetCore.Mvc;
using shopping.Models;

namespace shopping.Controllers
{
    public class ValidationController : Controller
    {
        private readonly shoppingContext _context;
        public ValidationController(shoppingContext shopping)
        {
            _context = shopping;
        }

        public JsonResult CheckUserName(string Username)
        {
            bool IsUsernameAvailable = !_context.Members.Any(u => u.UserName == Username);
            return Json(IsUsernameAvailable);
        }
      
        public JsonResult CheckEmail(string Email)
        {
            bool IsUsernameAvailable = !_context.Members.Any(u => u.Email == Email);
            return Json(IsUsernameAvailable);
        }

    }
}
