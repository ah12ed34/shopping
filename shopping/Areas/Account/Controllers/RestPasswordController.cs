using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shopping.Areas.Account.Models;
using shopping.Models;
using System.Security.Claims;

namespace shopping.Areas.Account.Controllers
{
    [Area("account")]
    [Route("Account/RestPassword"),Authorize]
    public class RestPasswordController : Controller
    {
        private readonly shoppingContext _Context;
        public RestPasswordController(shoppingContext shopping )
        {
            _Context = shopping;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(RestPassword restPassword)
        {
            int Id =Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                Member? member = _Context.Members.Where(u=>u.Id ==Id &&u.Password==restPassword.Password).FirstOrDefault();
                if (ModelState.IsValid)
                {
                   if(member == null)
                    {
                        TempData["Msg"] = "كلمة المرور غير صحيحة";
                        return View(restPassword);
                    }
                    else
                    {
                        member.Password = restPassword.Password;
                        member.PassLestDate = DateTime.UtcNow;
                        _Context.Members.Update(member);
                        _Context.SaveChanges();
                        return RedirectToAction(nameof(ProfileController.Index),"Profile",new {area="account"});
                    }
                }

            }catch (Exception ex)
            {
                TempData["Msg"] = ex.Message; 
            }
            return View(restPassword);
        }
        [Authorize(Roles = "admin")]
        [Route("Account/RestPassword/Admin/{UID}")]
        public IActionResult Admin(int UID)
        {
            RestPassword restPassword = new RestPassword();
            restPassword.Id = UID;
            return View(restPassword);
        }

        [Authorize(Roles ="admin"),HttpPost]
        [Route("Account/RestPassword/Admin")]
        public IActionResult Admin(RestPassword restPassword)
        {
            try
            {
                Member? member = _Context.Members.Where(u => u.Id == restPassword.Id).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    if (member != null)
                    {
                        member.Password = restPassword.Password;
                        member.PassLestDate = DateTime.UtcNow;
                        _Context.Members.Update(member);
                        _Context.SaveChanges();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(restPassword);
        }

        
    }
}
