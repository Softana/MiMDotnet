using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;
using MimMVC.Models.ViewModels;
using MimMVC.Utility;

namespace MimMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    //[Authorize(Roles = MimMVC.WC.AdminRole)]
    // [Authorize(Roles = WC.AdminRole + "," + WC.TeacherRole + "," + WC.AdminTeacherRole + WC.BankRole + WC.GeneralManagerRole)]
    public class InvoicesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMailService _mailService;

        public InvoicesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IMailService mailservice)
        {
            _userManager = userManager;
            this.context = context;
            _mailService = mailservice;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET /admin/invoices
        [Authorize(Roles = "Admin, LærerAdmin, Kasserer, Lærer, Leder")]
        public async Task<IActionResult> Index()
        {
            return View(await context.Invoices.OrderByDescending(x => x.Id).Include(x => x.UserCreate).ToListAsync());
        }

        //Get/admin/Invoices/create
        [Authorize(Roles = "Admin, LærerAdmin, Kasserer, Lærer, Leder")]
        public IActionResult Create()
        {
            ViewBag.InvoiceId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");
            return View();
        }

        // POST /admin/Invoices/create
        [HttpPost]
        [Authorize(Roles = "Admin, LærerAdmin, Kasserer, Lærer, Leder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                if (invoice.Title != null)
                {
                    invoice.Slug = invoice.Title.ToLower().Replace(" ", "-");
                }
                

                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == invoice.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Produktet findes allerede . .");
                    return View(invoice);
                }

                string imageName = "noimage.png";
                if (invoice.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "img/invoices");
                    imageName = Guid.NewGuid().ToString() + "_" + invoice.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await invoice.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                invoice.Image = imageName;
                
                context.Add(invoice);
                await context.SaveChangesAsync();
                
                TempData["Success"] = "Afregningen er blevet tilføjet . .!";
                var userLon = await _userManager.GetUserAsync(User);
                //// Send Email
                var pathLon = webHostEnvironment.WebRootPath + "/Templates/LonEmail.html";
                var html = System.IO.File.ReadAllText(pathLon);
                html = html.Replace("{{BrugernavnLon}}", userLon.FullName);
                html = html.Replace("{{InstruLon}}", userLon.Instruction);
                await _mailService.SendEmailOnlyBody("kasserer@musikimalling.dk", "Nyt bilag", html);

                return RedirectToAction("Index");               
            }
            return View(invoice);
        }

        // GET /admin/invoices/details/5
        [Authorize(Roles = "Admin, LærerAdmin, Kasserer, Lærer, Leder")]
        public async Task<IActionResult> Details(int id)
        {
            Invoice invoice = await context.Invoices.FirstOrDefaultAsync(x => x.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // GET /admin/Invoices/edit/5
        [Authorize(Roles = "Admin, LærerAdmin, Kasserer, Lærer, Leder")]
        public async Task<IActionResult> Edit(int id)
        {
            Invoice product = await context.Invoices.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View(product);
        }

        // POST /admin/invoices/edit
        [HttpPost]
        [Authorize(Roles = "Admin, LærerAdmin, Kasserer, Lærer, Leder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Invoice invoice)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                if (invoice.Title != null)
                {
                    invoice.Slug = invoice.Title.ToLower().Replace(" ", "-");
                }

                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == invoice.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Produktet findes allerede . .");
                    return View(invoice);
                }

                if (invoice.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "img/invoices");

                    if (!string.Equals(invoice.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, invoice.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + invoice.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await invoice.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    invoice.Image = imageName;
                }

                context.Update(invoice);
                await context.SaveChangesAsync();

                TempData["Success"] = "Bilaget er blevet Ændret . .!";

                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET /admin/Invoices/delete/5
        [Authorize(Roles = "Kasserer, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Invoice invoice = await context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                TempData["Error"] = "Bilaget findes ikke . .!";
            }
            else
            {
                //if (!invoice.Image.Equals("noimage.png"))
                //{
                //    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "img/invoices");
                //    string oldImagePath = Path.Combine(uploadsDir, invoice.Image);
                //    if (System.IO.File.Exists(oldImagePath))
                //    {
                //        System.IO.File.Delete(oldImagePath);
                //    }
                //}

                context.Invoices.Remove(invoice);
                await context.SaveChangesAsync();

                TempData["Success"] = "Bilaget er blevet slettet ..!";
            }
            return RedirectToAction("Index");
        }
    }   
}
