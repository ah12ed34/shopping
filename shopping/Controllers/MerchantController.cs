using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping.data.constants;
using shopping.data.Facade;
using shopping.Models;
using System.Security.Claims;

namespace shopping.Controllers
{
    [Authorize(Roles = "IsMer")]
    public class MerchantController : Controller
    {
        private readonly shoppingContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private  Merchant? _merchant;
        int id;
         
        public MerchantController(shoppingContext shoppingCont, IWebHostEnvironment webHost)
        {
           
            _context = shoppingCont;
            _hostingEnvironment = webHost ;
            


        }
        public IActionResult Index()
        {
            
            return View();
        }
        
       
        public IActionResult addProdect()
        {
            _merchant = _context.Members.Include(connectionString => connectionString.Merchant)
                        .Where(e => e.UserName == User.Identity.Name ).FirstOrDefault()?.Merchant;
            Prodect p = new Prodect();
            p.IdMerNavigation = _merchant;
            
            return View(p);
        }
        [HttpPost]
        public async Task<IActionResult> addProdect([Bind(nameof(Prodect.Name), nameof(Prodect.Price), 
            nameof(Prodect.Quantity), nameof(Prodect.VideoUrl),nameof(Prodect.Description)
            ,nameof(Prodect.IdMerNavigation),nameof(Prodect.IdMer))] Prodect p)
        {
            List<string> s = new List<string>();
            var File = new Ffile();
            //var path = "img/prodect/d/";
            try
            {
                p.IdMerNavigation = _context.Merchants.Where(e => e.Id ==Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) ).FirstOrDefault();
                if (ModelState.IsValid)
                {

                    foreach (var file in Request.Form.Files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var inputName = file.Name; // اسم input الذي تم رفع الملف منه
                            
                            if (inputName == "Himg")
                            {
                                  
                              s.Add(p.HomeImg = await File.UploadImage(file , CPath.pathImgHomeProduct));

                            }
                            else
                            {
                                
                                
                                switch (inputName)
                                {
                                    case "Img1":
                                       s.Add(p.Img1 = await File.UploadImage(file, CPath.pathImgProducts));
                                        break;
                                    case "Img2":
                                       s.Add( p.Img2 = await File.UploadImage(file, CPath.pathImgProducts));
                                        break;
                                    case "Img3":
                                       s.Add( p.Img3 = await File.UploadImage(file, CPath.pathImgProducts));
                                        break;

                                }
                            }
                            
                         
                        }

                    }
                         await _context.Prodects.AddAsync(p);

                         await  _context.SaveChangesAsync();
                }

               
            }
            catch (Exception ex)
            {

                File.DeleteImage(p.HomeImg, CPath.pathImgHomeProduct);
                File.DeleteImage(p.Img1, CPath.pathImgProducts);
                File.DeleteImage(p.Img2, CPath.pathImgProducts);
                File.DeleteImage(p.Img3, CPath.pathImgProducts);
                TempData["Msg"] = ex.Message ;
                TempData["Code"] = 3;
            }
            return View(p);
        } 
        
        public IActionResult ListProd() {
            id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return View(_context.Merchants.Include(e => e.Prodects).Where(e => e.Id == id).FirstOrDefault()); 
        }
        public async Task<IActionResult> DelProdect(int PID)
        {
            id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                Ffile ffile = new Ffile();
                Prodect? prodect = _context.Merchants.Include(e => e.Prodects).Where(e => e.Id == id).FirstOrDefault()?
                    .Prodects.Where(e => e.Id == PID).FirstOrDefault();
                if (prodect != null)
                {
                    if(_context.OrderDaitels.Where(e=>e.IdPro == PID).Any())
                    {
                        prodect.Quantity = 0;
                        prodect.IsPublishing = false;
                        _context.Prodects.Update(prodect);
                        await  _context.SaveChangesAsync();
                        TempData["Msg"] = "تم تصفير الكمية و الغاء نشر المنتج لاكن مازل عليك ايصال الطلابات المعلقة ";
                        return RedirectToAction(nameof(ListProd));
                    }
                    else
                    {
                        List<String> list = new List<String>();
                        list.Add(prodect.Img1);
                        list.Add(prodect.Img2);
                        list.Add(prodect.Img3);
                    
                    
                         ffile.DeleteImage(prodect.HomeImg, CPath.pathImgHomeProduct);
                         foreach (var item in list)
                         {
                             ffile.DeleteImage(item, CPath.pathImgProducts);
                         }
                         _context.Prodects.Remove(prodect);
                         await _context.SaveChangesAsync();
                         TempData["Msg"] = "تم حذف المنتج بنجاح";
                         TempData["Code"] = 0;


                    }
                    
                }
                else
                {
                    TempData["Msg"] = "لم يتم العثور على المنتج";
                    TempData["Code"] = 2;
                }

                return RedirectToAction(nameof(ListProd));
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                TempData["Code"] = 3;
            }
            return RedirectToAction(nameof(ListProd));
        }
        public IActionResult AddQty(int PID)
        {
            Prodect? prodect = _context.Prodects.Include(e=>e.IdMerNavigation).Where(e=>e.Id == PID).FirstOrDefault();
            if (!ChechProduct(prodect))
                return RedirectToAction(nameof(Index), "Home");

            return View(prodect);
        }
        [HttpPost]
        public IActionResult AddQty(Prodect p)
        {
          Prodect?  prop = _context.Prodects.Include(e => e.IdMerNavigation).Where(e => e.Id == p.Id).FirstOrDefault();
            if (!ChechProduct(prop)||p.Quantity==0||prop.Quantity-(-1*p.Quantity)<=0)
                return RedirectToAction(nameof(Index), "Home");
            try
            {
                  prop.Quantity += p.Quantity;
                  _context.Prodects.Update(prop);
                  _context.SaveChanges();

              return  RedirectToAction(nameof(ListProd));


            }catch (Exception ex) { }



            return View(prop);
        }
        public bool ChechProduct(Prodect? prodect)
        {
            if (prodect != null)
            {
                if (prodect.IdMer != Convert.ToInt32(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)))
                {
                    prodect = null;
                    TempData["Msg"] = "هذا المنتج ليس تابع لك";
                    TempData["Code"] = 3;
                    return false;
                }

            }
            else
            {
                TempData["Msg"] = "لم يتم العثور على المنتج";
                TempData["Code"] = 3;
                return false;
            }
            return true;
        }
    }
}
