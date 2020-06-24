using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraniteCore2_1.Data;
using GraniteCore2_1.Models.ViewModels;
using GraniteCore2_1.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteCore2_1.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hstEnv;

        [BindProperty]
        public ProductViewModel ProductViewModel { get; set; }

        public ProductController(ApplicationDbContext context, HostingEnvironment hstEnv)
        {
            _context = context;
            _hstEnv = hstEnv;
            ProductViewModel = new ProductViewModel()
            {
                Product = new Models.Product(),
                ProductTypes = _context.ProductTypes.ToList(),
                SpecialTags = _context.SpecialTags.ToList()
            };
        }
        public async Task<IActionResult> Index()
        {
            var product = _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag);
            return View(await product.ToListAsync());
        }
        public IActionResult Create()
        {
            return View(ProductViewModel);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(ProductViewModel);
            }
            _context.Products.Add(ProductViewModel.Product);
            await _context.SaveChangesAsync();

            var webRootPath = _hstEnv.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var dbProduct = _context.Products.Find(ProductViewModel.Product.Id);

            if (files.Count != 0)
            {
                var uplodes = Path.Combine(webRootPath, SD.ImageFolder);
                var extention = Path.GetExtension(files[0].FileName); 

                using(var fileStream=new FileStream(Path.Combine(uplodes, ProductViewModel.Product.Id + extention),FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                dbProduct.Image = @"\" + SD.ImageFolder + @"\" + ProductViewModel.Product.Id + extention;
            }
            else
            {
                var uplodes = Path.Combine(webRootPath,SD.ImageFolder+@"\" +SD.DefaultProductImage);
                System.IO.File.Copy(uplodes, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductViewModel.Product.Id + ".jpg");
                dbProduct.Image = @"\" + SD.ImageFolder + @"\" + ProductViewModel.Product.Id + ".jpg";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductViewModel.Product =await _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).SingleOrDefaultAsync(s => s.Id == id);

            if (ProductViewModel.Product == null)
            {
                return NotFound();
            }

            return View(ProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _hstEnv.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var dbProduct = _context.Products.Where(m => m.Id == productViewModel.Product.Id).FirstOrDefault();

                if(files.Count>0 && files[0]!=null) 
                {
                    var uplodes = Path.Combine(webRootPath, SD.ImageFolder);
                    var newExtention = Path.GetExtension(files[0].FileName);
                    var oldExtention = Path.GetExtension(dbProduct.Image);

                    if(System.IO.File.Exists(Path.Combine(uplodes, productViewModel.Product.Id + oldExtention)))
                    {
                        System.IO.File.Delete(Path.Combine(uplodes, productViewModel.Product.Id + oldExtention)); 
                    }
                    using (var fileStream = new FileStream(Path.Combine(uplodes, ProductViewModel.Product.Id + newExtention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productViewModel.Product.Image = @"\" + SD.ImageFolder + @"\" + ProductViewModel.Product.Id + newExtention;
                }
                if (productViewModel.Product.Image != null)
                {
                    dbProduct.Image = productViewModel.Product.Image ;
                }
                dbProduct.Name = productViewModel.Product.Name;
                dbProduct.Price = productViewModel.Product.Price;
                dbProduct.IsAvailable = productViewModel.Product.IsAvailable;
                dbProduct.ProductTypeId = productViewModel.Product.ProductTypeId;
                dbProduct.SpecialTagId = productViewModel.Product.SpecialTagId;
                dbProduct.ShadeColor = productViewModel.Product.ShadeColor;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(productViewModel);
            }      
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductViewModel.Product = await _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).SingleOrDefaultAsync(s => s.Id == id);

            if (ProductViewModel.Product == null)
            {
                return NotFound();
            }

            return View(ProductViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductViewModel.Product = await _context.Products.Include(pt => pt.ProductType).Include(st => st.SpecialTag).SingleOrDefaultAsync(s => s.Id == id);

            if (ProductViewModel.Product == null)
            {
                return NotFound();
            }

            return View(ProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ProductViewModel productViewModel)
        {
            var webRootPath = _hstEnv.WebRootPath;
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
            {
                return NotFound();
            }
            else
            {
                var uplodes = Path.Combine(webRootPath, SD.ImageFolder);
                var extention = Path.GetExtension(dbProduct.Image);

                if (System.IO.File.Exists(Path.Combine(uplodes, productViewModel.Product.Id + extention)))
                {
                    System.IO.File.Delete(Path.Combine(uplodes, productViewModel.Product.Id + extention));
                }
                _context.Products.Remove(dbProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
        }
    }
}