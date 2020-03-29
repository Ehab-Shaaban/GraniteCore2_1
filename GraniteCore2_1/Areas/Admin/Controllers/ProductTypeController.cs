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
    public class ProductTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypeController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(model: _context.ProductTypes.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(productType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var productType = await _context.ProductTypes.FindAsync(id);
                if (productType == null)
                {
                    return NotFound();
                }
                return View(productType);
            }
            return NotFound();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Update(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(productType);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var productType = await _context.ProductTypes.FindAsync(id);
                if (productType == null)
                {
                    return NotFound();
                }
                return View(productType);
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var productType = await _context.ProductTypes.FindAsync(id);
                if (productType == null)
                {
                    return NotFound();
                }
                return View(productType);
            }
            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
