using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteCore2_1.Data;
using GraniteCore2_1.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GraniteCore2_1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SpecialTagController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(model: _context.SpecialTags.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var specialTag =await _context.SpecialTags.FindAsync(id);
                return View(specialTag);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _context.Update(specialTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var specialTag = await _context.SpecialTags.FindAsync(id);
                return View(specialTag);
            }
            return NotFound();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Details(SpecialTag specialTag)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Update(specialTag);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(specialTag);
        //}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var specialTag = await _context.SpecialTags.FindAsync(id);
                return View(specialTag);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var specialTag = await _context.SpecialTags.FindAsync(id);
            _context.Remove(specialTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }
    }
}
