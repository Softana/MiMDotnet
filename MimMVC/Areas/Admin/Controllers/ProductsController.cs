using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;

namespace MimMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = MimMVC.WC.AdminRole)]
    public class ProductsController : Controller
    {
	    private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
	    {
		    this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET /admin/products        
        public async Task<IActionResult> Index()
		{
			return View(await context.Products.OrderByDescending(x => x.Id).Include(x => x.Category).ToListAsync());
		}

        //Get/admin/Products/create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
		{
			ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");
			return View();
		}

        // POST /admin/products/create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Title.ToLower().Replace(" ", "-");

                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Produktet findes allerede . .");
                    return View(product);
                }

                string imageName = "noimage.png";
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "img/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                product.Image = imageName;

                context.Add(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "Produktet er blevet tilføjet . .!";

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET /admin/products/details/5
        public async Task<IActionResult> Details(int id)
		{
			Product product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

        // GET /admin/products/edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            return View(product);
        }

        // POST /admin/products/edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Title.ToLower().Replace(" ", "-");

                var slug = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "img/products");

                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }

                context.Update(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "Produktet er blevet ændret . .!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET /admin/products/delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = "Produktet findes ikke . .!";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "img/products");
                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "Produktet er blevet slettet ..!";
            }

            return RedirectToAction("Index");
        }
    }
}
