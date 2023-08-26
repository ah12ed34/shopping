using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping.Areas.Account.Models;
using shopping.Models;
using System.Security.Claims;

namespace shopping.Areas.Account.Controllers
{
    [Area(nameof(Account))]
    [Route("Account/Login")]
    public class LoginController : Controller
    {
        private readonly shoppingContext _context;
        public LoginController(shoppingContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Msg"] = "انت مسجل بالفعل";
                TempData["code"] = 3;
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Login vM)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Msg"] = "انت مسجل بالفعل";
                TempData["code"] = 3;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Member? login = _context.Members.Where(e => (e.UserName == vM.UserName || e.Email == vM.UserName) && e.Password == vM.Password).Include(e => e.IdRs).Include(e => e.Merchant).FirstOrDefault();
                        if (login != null)
                        {

                            var claims = new List<Claim>
                        {
                          new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                          new Claim(ClaimTypes.Name, login.UserName),
                          new Claim(ClaimTypes.Email, login.Email),
                         
                          
                          
                          
                         // new Claim(ClaimTypes.Role, login.UserName),
                          // يمكن إضافة المزيد من المعلومات المطلوبة هنا
                         };
                            if (login.Profile != null)
                                claims.Add(new Claim("profile", login.Profile));
                            foreach (var item in login.IdRs)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, item.Name));
                            }

                            if (login.Merchant != null && login.Merchant.IsActvity == true)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "IsMer"));
                            }

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = vM.RememberMe
                            };
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                 new ClaimsPrincipal(claimsIdentity), authProperties);

                            User.AddIdentity(claimsIdentity);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "البيانات غير صحيحة");
                        }

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    //var signInResult = await _signInManager.PasswordSignInAsync(vM.UserName, vM.Password, vM.RememberMe, false);
                    //var signInResult = await _signInManager.PasswordSignInAsync(vM.UserName, vM.Password, vM.RememberMe, false);
                    //if(signInResult.Succeeded)
                    //{

                    //    return RedirectToAction("Index", "Home");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, "البينات غير صحيح");
                    //    return View();
                    //}
                }
                return View();
            }
        }
    }
}
