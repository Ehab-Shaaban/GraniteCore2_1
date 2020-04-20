using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraniteCore2_1.Models;
using GraniteCore2_1.Data;
using Microsoft.EntityFrameworkCore;
using GraniteCore2_1.Extentions;
using Microsoft.AspNetCore.Http;

namespace GraniteCore2_1.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var productList = await _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).ToListAsync();
            return View(productList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).Where(l => l.Id == id).FirstOrDefaultAsync();

            return View(product);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            var shoppingCartList = HttpContext.Session.Get<List<int>>("shoppingCartSession");

            if (shoppingCartList == null)
            {
                shoppingCartList = new List<int>();
            }

            shoppingCartList.Add(id);
            HttpContext.Session.Set("shoppingCartSession", shoppingCartList);

            return RedirectToAction("Index","Home",new { Area = "Customer" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var shoppingCartList = HttpContext.Session.Get<List<int>>("shoppingCartSession");

            if (shoppingCartList.Count > 0)
            {
                if (shoppingCartList.Contains(id))
                {
                   shoppingCartList.Remove(id);
                }
            }
            
            HttpContext.Session.Set("shoppingCartSession", shoppingCartList);

            return RedirectToAction("Index", "Home", new { Area = "Customer" });
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
