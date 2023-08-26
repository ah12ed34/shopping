using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shopping.Models;
using shopping.data.Repositories;
using Microsoft.EntityFrameworkCore;
using shopping.ViewModels;
using shopping.data.constants;

namespace shopping.Controllers
{
    [Authorize(Roles ="admin")]
    public class DashboardController : Controller
    {
        public readonly shoppingContext _Context;
        public readonly IRepository<Member> _RepositoryMem;
        public readonly IRepository<Merchant> _RepositoryMer;
        public readonly IRepository<Prodect> _RProduct;
        public DashboardController(shoppingContext context,IRepository<Member> repository
            ,IRepository<Merchant> repositoryM ,IRepository<Prodect> repository1 )
        {
            _Context = context;
            _RepositoryMem = repository;
            _RepositoryMer = repositoryM;
            _RProduct = repository1;
        }
        public IActionResult Index()
        {
            DashboardVM vm = new DashboardVM();
            vm.prodect = _Context.Prodects ;
            vm.members = _Context.Members ;
            vm.merchant = _Context.Merchants ;
            vm.orderDaitels = _Context.OrderDaitels ;
            vm.orders = _Context.Orders ;
            return View(vm);
        }

        //string Page = nameof(PagerModel.CurrentPage);
        public IActionResult Dashboard()
        {
            return View();
        }
        
        public async Task<ActionResult> ListUser(int page = 1, int pageSize = 5,string Sort = ""
            ,string Filter = "",string Search = "")
        {
            IQueryable<Member> Iq =_Context.Members;
            PagerModel pager;
            switch (Sort)
            {
                case "Name":
                  Iq =  Iq.OrderBy(e => e.Fname);
                    break;
                case "NameDes":
                    Iq = Iq.OrderByDescending(e => e.Fname);
                    break;
                case "UN":
                    Iq = Iq.OrderBy(e=>e.UserName);
                    break;
                case "UNDes":
                    Iq = Iq.OrderByDescending(e => e.UserName);
                    break;
            }
            if( Search != "")
            {
                switch (Filter)
                {
                    case "username":
                        Iq = Iq.Where(e=>e.UserName.Contains(Search));
                        break;
                    case "email":
                        Iq = Iq.Where(e=> e.Email.Contains(Search));
                        break;

                    case "Phone":
                        Iq = Iq.Where(e=>e.Phone.ToString().Contains(Search));
                        break;
                    default:
                        Iq = Iq.Where(e=>e.Fname.Contains(Search)|e.Mname.Contains(Search)|e.Lname.Contains(Search));
                        break;
                }
            }
            ViewBag.Peger = pager = new PagerModel(Iq.Count(), page, pageSize);
            pager.Sort = Sort;
            pager.Filter = Filter;
            pager.Search = Search;
            return View(await Iq.Skip((page-1)*pageSize).Take(pageSize).ToListAsync());

        }
        public async Task<IActionResult> ListMer(int page = 1 ,int pageSize = 10)
        {
            List<Merchant> members = await _RepositoryMer.Get(page, pageSize).Include(e=>e.IdNavigation).ToListAsync();
            ViewBag.TotalItems = _Context.Merchants.Count();
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            //ViewBag.SName = ;
            return View(members);
        }
        public IActionResult ListProduct(int page = 1, int pageSize = 10)
        {
            List<Prodect> members =  _RProduct.Get(page, pageSize).Include(e=>e.IdMerNavigation).ToList();
            ViewBag.TotalItems = _Context.Prodects.Count();
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.Peger = new PagerModel(_Context.Prodects.Count(), page, pageSize);
            return View(members);
        }
        public IActionResult ListProductDis(int page =1,int pageSize = 10)
        {
            List<Prodect> members = _RProduct.Get(page, pageSize).Include(e => e.IdMerNavigation).Where(e=>e.IsPublishing != true).ToList();
            ViewBag.TotalItems = _Context.Prodects.Count();
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            return View(members);
        }
        public IActionResult actOrDis(int MID)
        {
            try
            {Merchant m =  _RepositoryMer.Get(MID);
            
                 m.IsActvity = !m.IsActvity ?? true;
            
            if(_RepositoryMer.Update(m))
            
                TempData["Msg"] = "Done";
            }
            catch(Exception ex)
            {
                TempData["Msg"] = ex.Message;
                TempData["Code"] = 3; 
            }
           
           return RedirectToAction(nameof(ListMer));
        }
            public IActionResult actOrDisProd(int PID)
        {
            try
            {Prodect m =  _RProduct.Get(PID);
                    if(m != null)
                {
                    m.IsPublishing = !m.IsPublishing ?? true ;
            
                if(_RProduct.Update(m))
                TempData["Msg"] = "Done";
                }
               
            }
            catch(Exception ex)
            {
                TempData["Msg"] = ex.Message;
                TempData["Code"] = 3; 
            }
           
           return RedirectToAction(nameof(ListProductDis));
        }

    }
}
