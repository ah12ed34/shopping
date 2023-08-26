using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping.Models;
using shopping.ViewModels;

namespace shopping.Areas.Account.Controllers
{
    [Area(nameof(Account))]
    [Route("account/AddMerchant")]
    public class AddMerchantController : Controller
    {
        private readonly shoppingContext _context;
        public AddMerchantController(shoppingContext shopping)
        {
            _context = shopping;
        }

        [Authorize]
        public IActionResult Index()
        {
            var ss = _context.Merchants.Include(e => e.IdNavigation).Where(e => e.IdNavigation.UserName == User.Identity.Name).FirstOrDefault();
            if (ss != null && ss.Id != null)
            {
                TempData["Msg"] = "انت تاجر بالفعل";
                TempData["code"] = 2;
                return RedirectToAction(nameof(Index), "Home");
            }
            MerchantVM merchantVM = new MerchantVM();
            if (User.Identity.IsAuthenticated)
            {
                merchantVM.IdNavigation = (Member)_context.Members.Where(e => e.UserName == User.Identity.Name).FirstOrDefault();

            }
            return View(merchantVM);

        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection m)
        {
            //if(m != null)
            //{
            //    TempData["Msg"] = $"id {m.Id.ToString()} , idN = {m.IdNavigation.Id.ToString()}";
            //    return RedirectToAction(nameof(Index), "Home");
            //}

            try
            {
                if (ModelState.IsValid)
                {
                    Merchant merchant = new Merchant
                    {
                        Id = Convert.ToInt32(m["Id"]),
                        TradeName = m["TradeName"],
                        Locaction = m["Locaction"],


                    };
                    merchant.Id = Convert.ToInt32(m["Id"]);
                    if (User.Identity.IsAuthenticated)
                    {
                        var s = Convert.ToDecimal(m["Tax"]) / 100;
                        merchant.Tax = (double?)Math.Round(s, 2);
                        var res = await _context.Merchants.AddAsync(merchant);

                        if (res != null)
                        {
                            await _context.SaveChangesAsync();
                            TempData.Add("Msg", "تم إنشاء حساب تاجر ، في انتظار تفعيل الحساب من قبل مسؤول النظام");
                            TempData["code"] = 0;
                            return RedirectToAction(nameof(Index), "Home");
                        }

                    }
                    else
                    {
                        // IsAuth 

                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(new Merchant { IdNavigation = (Member)_context.Members.Where(e => e.UserName == User.Identity.Name).FirstOrDefault() });

        }
    }
}
