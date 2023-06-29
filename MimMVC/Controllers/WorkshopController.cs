using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Controllers
{
    public class WorkshopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public WorkshopController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = webHostEnvironment;
        }




        public IActionResult Index()
        {
            var workshops = _context.Workshops.Include(x => x.TilmeldteBrugere).ThenInclude(x => x.user).ToList();
            return View(workshops);
        }

        public IActionResult Edit(int id)
        {
            var workshop = _context.Workshops.Include(x => x.TilmeldteBrugere).ThenInclude(x => x.user).FirstOrDefault(x => x.Id == id);
            return View(workshop);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkShop workShop)
        {

            if (workShop.File != null)
            {

                string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/invitation/");

                var imageName = workShop.Id + "_" + workShop.File.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await workShop.File.CopyToAsync(fs);
                fs.Close();
                workShop.FileName = imageName;
            }

            _context.Update(workShop);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkShop workShop)
        {

            var work = _context.Workshops.Add(workShop);
            _context.SaveChanges();
            string imageName = "noimage.png";

            if (workShop.File != null)
            {
                string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/invitation/");

                imageName = work.Entity.Id + "_" + workShop.File.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await workShop.File.CopyToAsync(fs);
                fs.Close();
                workShop.FileName = imageName;

            }

            workShop.FileName = imageName;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var workshop = _context.Workshops.Include(x => x.TilmeldteBrugere).FirstOrDefault(x => x.Id == id);
            var indmeldelser = _context.WorkShopIndmeldelser.Where(x => x.workShop == workshop).ToList();
            _context.WorkShopIndmeldelser.RemoveRange(indmeldelser);
            _context.Workshops.Remove(workshop);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Tilmeld(int id)
        {
            var workshop = _context.Workshops.Include(x => x.TilmeldteBrugere).FirstOrDefault(x => x.Id == id);

            if (workshop is null) return View("Index");

            var user = await _userManager.GetUserAsync(User);

            var indmeldelse = new TilmeldDto() { WorkshopId = id };
            // workshop.TilmeldteBrugere.Add(indmeldelse);    
            return View(indmeldelse);
        }

        [HttpPost]
        public async Task<IActionResult> Tilmeld(TilmeldDto tilmeld)
        {
            var workshop = _context.Workshops.Include(x => x.TilmeldteBrugere).FirstOrDefault(x => x.Id == tilmeld.WorkshopId);
            var user = await _userManager.GetUserAsync(User);

            var tilmeldelse = new WorkShopIndmeldelse() { Gæster = tilmeld.guests, user = user, workShop = workshop };

            if (workshop.TilmeldteBrugere.Where(x => x.user == user).Count() == 0)
            {
                workshop.TilmeldteBrugere.Add(tilmeldelse);
                workshop.AntalGæster += tilmeld.guests;
                workshop.TotalTilmeldt += tilmeld.guests + 1;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Afmeld(int id)
        {
            var workshop = _context.Workshops.Include(x => x.TilmeldteBrugere).ThenInclude(x => x.user).FirstOrDefault(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            var tilmeldelse = workshop.TilmeldteBrugere.Where(x => x.user.Id == user.Id).FirstOrDefault();

            if (tilmeldelse is not null)
            {
                _context.WorkShopIndmeldelser.Remove(tilmeldelse);
                workshop.TotalTilmeldt -= tilmeldelse.Gæster + 1;
                workshop.AntalGæster -= tilmeldelse.Gæster;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
