using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteCore2_1.Data;
using GraniteCore2_1.Extentions;
using GraniteCore2_1.Models;
using GraniteCore2_1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteCore2_1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
            ShoppingCartViewModel = new ShoppingCartViewModel
            {
                ProductList = new List<Models.Product>()
            };
        }

        public async Task<IActionResult> Index()
        {
            var shoppingCartList = HttpContext.Session.Get<List<int>>("shoppingCartSession");
            if (shoppingCartList.Count > 0)
            {
                foreach (var item in shoppingCartList)
                {

                    Product prod = _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).Where(p => p.Id == item).FirstOrDefault();
                    ShoppingCartViewModel.ProductList.Add(prod);
                }
            }
            return View(ShoppingCartViewModel);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost()
        {
            var shoppingCartList = HttpContext.Session.Get<List<int>>("shoppingCartSession");
            ShoppingCartViewModel.Appointment.AppointmentDate = ShoppingCartViewModel.Appointment.AppointmentDate
                .AddHours(ShoppingCartViewModel.Appointment.AppointmentTime.Hour)
                .AddMinutes(ShoppingCartViewModel.Appointment.AppointmentTime.Minute);

            Appointment appointment = ShoppingCartViewModel.Appointment;
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            foreach (int productId in shoppingCartList)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment
                {
                    AppointmentId = appointment.Id,
                    ProductId = productId
                };
                _context.ProductsSelectedForAppointments.Add(productsSelectedForAppointment);
            }
            await _context.SaveChangesAsync();
            shoppingCartList = new List<int>();
            HttpContext.Session.Set("shoppingCartSession", shoppingCartList);


            return RedirectToAction("AppointmentConfirm", "ShoppingCart", new { id = appointment.Id });
        }
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
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> AppointmentConfirm(int id)
        {
            ShoppingCartViewModel.Appointment = _context.Appointments.Where(l => l.Id == id).FirstOrDefault();
            var proAppo = _context.ProductsSelectedForAppointments.Where(i => i.AppointmentId == id).ToList();
            foreach(var item in proAppo)
            {
                ShoppingCartViewModel.ProductList.Add(_context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).Where(i=>i.Id==item.ProductId).FirstOrDefault());
            }
            return View(ShoppingCartViewModel);
        }
    }
}