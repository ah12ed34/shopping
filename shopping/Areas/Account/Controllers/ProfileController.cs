using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shopping.data.Facade;
using shopping.Models;
using System.Security.Claims;

namespace shopping.Areas.Account.Controllers
{
    [Area(nameof(Account))]
    [Authorize]
    [Route("Account/Profile")]
    public class ProfileController : Controller
    {
        private readonly shoppingContext _context;
        private const string pathProfile = "img/profile/";
        public ProfileController(shoppingContext context)
        {
            _context = context;
        }
        
        
        public IActionResult Index()
        {
            int id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Member member = _context.Members.Where(e=>e.Id == id).FirstOrDefault();
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Member? m)
        {
              m =  _context.Members.Where(m => m.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            var f = new Ffile();
            var ifile = Request.Form.Files;
            if (ifile != null)
            {
                if(m.Profile == null)
               m.Profile=await f.UploadImage(ifile[0], pathProfile);
                else
                {
                    f.DeleteImage(m.Profile, pathProfile);
                    m.Profile = await f.UploadImage(ifile[0], pathProfile);
                }
                _context.Members.Update(m);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Home");
        }

    }
}
