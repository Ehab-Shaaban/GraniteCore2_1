using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteCore2_1.Data;
using GraniteCore2_1.Models;
using GraniteCore2_1.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GraniteCore2_1.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")] 
    public class AdminUsersController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public AdminUsersController(ApplicationDbContext context)
        {
            _context = context;   
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_context.ApplicationUsers.ToList());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id==null || id.Trim().Length==0)
            {
                return NotFound();
            }

            var dbUser = await _context.ApplicationUsers.FindAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }
            return View(dbUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(string id,ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            
            if (ModelState.IsValid)
            {
                var dbUser = _context.ApplicationUsers.Where(u=>u.Id==id).FirstOrDefault();
                dbUser.Name = applicationUser.Name;
                dbUser.PhoneNumber = applicationUser.PhoneNumber;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(applicationUser);
            }
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }

            var dbUser = await _context.ApplicationUsers.FindAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }
            return View(dbUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeletePost(string id)
        {
            
                var dbUser = _context.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();

                dbUser.LockoutEnd = DateTime.Now.AddYears(1000);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
        }
    }
}
