using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shopping.Models;
using shopping.ViewModels;

namespace shopping.Controllers
{
    [Authorize(Roles = "role")]
    public class RolesController : Controller
    {
        private readonly shoppingContext _context;
         
        public RolesController(shoppingContext context)
        {
            _context = context;
        }
        public IActionResult CR(int id)
        {
            if(id == null||id==0)
            {
                return NotFound();

            }
            TempData["uid"] = id;
            //List<crVM> crVMs = new List<crVM>();
            //foreach(var item in _context.Roles.ToList())
            //{
            //    crVMs.Add( (crVM) item);
            //}
             
            return View(_context.Roles.Include(e=>e.IdMs).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> CR(IFormCollection f) {
            
            //string[] ids = Request.Form["Id[]"];
            //string[] IsRole = Request.Form["IsRole[]"];
        //    TempData["Msg"] = IsRole[0].ToString();
            try
            {
                
                int uid = int.Parse(f["uid"]);
                //var user = _context.Members.Include(e => e.IdRs).Where(e => e.Id == f["uid"]).FirstOrDefault();
                var roles = _context.Roles.Include(e=>e.IdMs);
                //roles.FirstOrDefault().IdMs.Remove(user);
                    _context.Database.ExecuteSqlRaw($"delete Rolesm where IdM = {uid}");
                foreach (var item in roles.ToList())
                   { 
                    if (isB(f[$"IsRole_{item.Name}"]))
                        {
                        //user.IdRs.Add(_context.Roles.Where(e => e.Id == f[$"Id[{i}]"]).FirstOrDefault());
                        //roles.Where(e => e.Id == f[$"Id_{i}"]).FirstOrDefault().IdMs.Add(user);
                        
                        _context.Database.ExecuteSqlRaw($"exec addrole {uid} , {item.Id} ");
                        }
                    }
                 _context.SaveChanges();
                
                //var user = _context.Members.Include(e => e.IdRs).Where(e => e.Id == 6).FirstOrDefault();
                //var roles = _context.Roles.Include(e => e.IdMs).Where(e=>e.Id==2).FirstOrDefault();
                //roles.IdMs.Add(user);
                
                return RedirectToAction(nameof(ListUser));
            }catch(Exception ex)
            {
                TempData["Msg"]=ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
              TempData["uid"] = f["uid"];
             // TempData["idT"] = f["IsRole[0]"].ToString();
            return View(_context.Roles.Include(e => e.IdMs).ToList());
            }   
        // GET: Roles
        public async Task<IActionResult> Index()
        {
              return View(await _context.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Role role)
        {
            
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }
        public IActionResult ListUser()
        {

            return View(_context.Members.Include(e=>e.IdRs).ToList());
        }
        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'shoppingContext.Roles'  is null.");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       public bool isB(object v)
        {
            if(v.ToString() == "on")
            {
             return true;
            }else
            return false;

        }

        private bool RoleExists(int id)
        {
          return _context.Roles.Any(e => e.Id == id);
        }
    }
}
