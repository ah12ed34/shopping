using Microsoft.AspNetCore.Mvc;
using shopping.Areas.Account.Models;
using shopping.Models;
using shopping.ViewModels;

namespace shopping.Areas.Account.Controllers
{
    [Area(nameof(Account))]
    [Route("Account/Register")]
    public class RegisterController : Controller
    {
        private readonly shoppingContext _context;
        public RegisterController(shoppingContext shopping)
        {
            _context = shopping;
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                TempData["Msg"] = "انت مسجل بالفعل";
                TempData["code"] = 3;
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Register member)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Msg"] = "انت مسجل بالفعل";
                TempData["code"] = 3;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Member m = member;
                        var addu = await _context.Members.AddAsync(m);
                        if (addu != null)
                        {
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index), "Login", new { Area = "Account" });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "خطا في اضافات البينات");
                        }
                        //var result = await _userManager.CreateAsync(m,member.Password);
                        //if (result.Succeeded)
                        //{
                        //    return RedirectToAction(nameof(login),"account");
                        //}
                        //else
                        //{
                        //    foreach(var error in result.Errors)
                        //    ModelState.AddModelError(string.Empty, error.Description);
                        //}
                    }

                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                        ex = ex.InnerException;

                    }
                    
                }

                return View();
            }
        }
       

    }
}
