using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;

namespace MimMVC.Controllers
{
    public class FrontPagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FrontPagesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

      
        #region MyRegion

        // GET: FrontPages
        public async Task<IActionResult> PostNewContent_Index()
        {
            return View(await _context.FontPages.ToListAsync());
        }

        // GET: FrontPages/Create
        public IActionResult PostNewContent_Create()
        {
            return View();
        }

        // POST: FrontPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostNewContent_Create([Bind("Id,PostTitle,PostContent,ImageFile")] FrontPage postNewContent)
        {
            if (ModelState.IsValid)
            {
                
                string imageName = "noimage.png";
                if (postNewContent.ImageFile != null)
                {
                    // https://www.youtube.com/watch?v=QpJvqiHl1Fo
                    // Save image to wwwroot/img/post
                    string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/post");
                    imageName = Guid.NewGuid().ToString() + "_" + postNewContent.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await postNewContent.ImageFile.CopyToAsync(fs);
                    fs.Close();
                }
                //Insert record
                postNewContent.ImageName = imageName;

                // Set datetime
                postNewContent.Date = DateTime.Now;

                _context.Add(postNewContent);
                await _context.SaveChangesAsync();
                return RedirectToAction("PostNewContent_Index");
            }
            return View(postNewContent);
        }

        // GET: FrontPages/Edit/5
        public async Task<IActionResult> PostNewContent_Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postNewContent = await _context.FontPages.FindAsync(id);
            if (postNewContent == null)
            {
                return NotFound();
            }
            return View(postNewContent);
        }

        // POST: FrontPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostNewContent_Edit(int id, [Bind("Id,PostTitle,PostContent,ImageName,ImageFile")] FrontPage postNewContent)
        {
            if (id != postNewContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objFromDb = await _context.FontPages.FindAsync(id);
                    // --- Edit Img ---
                    if (postNewContent.ImageFile != null)
                    {
                        string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/post");

                        if (objFromDb.ImageName != "noimage.png" && objFromDb.ImageName != null)
                        {
                            string oldImagePath = Path.Combine(uploadsDir, objFromDb.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string imageName = Guid.NewGuid().ToString() + "_" + postNewContent.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await postNewContent.ImageFile.CopyToAsync(fs);
                        fs.Close();
                        postNewContent.ImageName = imageName;
                        objFromDb.ImageName = postNewContent.ImageName;
                    }

                    objFromDb.PostTitle = postNewContent.PostTitle;
                    objFromDb.PostContent = postNewContent.PostContent;


                    _context.Update(objFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrontPageExists(postNewContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PostNewContent_Index");
            }
            return View(postNewContent);
        }


        // GET: FrontPages/Delete/5
        public async Task<IActionResult> PostNewContent_Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postNewContent = await _context.FontPages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postNewContent == null)
            {
                return NotFound();
            }

            return View(postNewContent);
        }

        // POST: FrontPages/Delete/5
        [HttpPost, ActionName("PostNewContent_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostNewContent_DeleteConfirmed(int id)
        {
            var postNewContent = await _context.FontPages.FindAsync(id);

            if (!postNewContent.ImageName.Equals("noimage.png"))
            {
                //delete image from wwwroot/img/frontPage
                string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/post");
                string oldImagePath = Path.Combine(uploadsDir, postNewContent.ImageName);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                //Delete the record
            }


            _context.FontPages.Remove(postNewContent);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostNewContent_Index");
        }




        #endregion

        // ------------------- Banner ------------------
        #region MyRegion

        // GET: FrontPages
        public async Task<IActionResult> Banner_Index()
        {
            return View(await _context.FontPages.ToListAsync());
        }

        public async void setFirst_Banner_Index(int id )
        {
            var something = _context.FontPages.ToList();
            
            foreach (var item in something)
            {
                if (item.Id == id)
                {
                    item.FirstBanner = true;
                }
                else
                {
                    item.FirstBanner = false;
                }
            }

            _context.UpdateRange(something);
            _context.SaveChanges();
        }

        // GET: FrontPages/Create
        public IActionResult Banner_Create()
        {
            return View();
        }
        // POST: FrontPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Banner_Create(FrontPage banner)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (banner.ImageFile != null)
                {
                    // https://www.youtube.com/watch?v=QpJvqiHl1Fo
                    // Save image to wwwroot/img/post
                    string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/layout/banner");
                    imageName = Guid.NewGuid().ToString() + "_" + banner.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await banner.ImageFile.CopyToAsync(fs);
                    fs.Close();
                }
                //Insert record
                banner.ImageName = imageName;

                // Set datetime
                banner.Date = DateTime.Now;

                if (banner.BannerButtonName == null)
                {
                    banner.BannerButtonName = "Link";
                }

                _context.Add(banner);
                await _context.SaveChangesAsync();

                return RedirectToAction("Banner_Index");
            }
            return View(banner);
        }

        // GET: FrontPages/Edit/5
        public async Task<IActionResult> Banner_Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.FontPages.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: FrontPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Banner_Edit(int id, FrontPage banner)
        {
            
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var objFromDb = await _context.FontPages.FindAsync(id);
                try
                {

                    // --- Edit Img ---
                    if (banner.ImageFile != null)
                    {
                        string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/layout/banner");
                         
                        if (objFromDb.ImageName != "noimage.png" && objFromDb.ImageName != null)
                        {

                            string oldImagePath = Path.Combine(uploadsDir, objFromDb.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string imageName = Guid.NewGuid().ToString() + "_" + banner.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await banner.ImageFile.CopyToAsync(fs);
                        fs.Close();
                        banner.ImageName = imageName;
                        objFromDb.ImageName = banner.ImageName;
                    }
                    objFromDb.BannerContent = banner.BannerContent;
                    objFromDb.BannerButtonLink = banner.BannerButtonLink;
                    objFromDb.BannerButtonName = banner.BannerButtonName;
                    

                    _context.Update(objFromDb);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrontPageExists(banner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Banner_Index");
            }
            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Banner_EditImg(int id, FrontPage banner)
        {

            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var objFromDb = await _context.FontPages.FindAsync(id);
                try
                {

                    // --- Edit Img ---
                    if (objFromDb.ImageName != null)
                    {
                        string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/layout/banner");

                        if (!objFromDb.ImageName.Equals("noimage.png"))
                        {
                            string oldImagePath = Path.Combine(uploadsDir, objFromDb.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string imageName = Guid.NewGuid().ToString() + "_" + banner.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await banner.ImageFile.CopyToAsync(fs);
                        fs.Close();
                        objFromDb.ImageName = imageName;
                    }


                    _context.Update(objFromDb);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrontPageExists(banner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Banner_Index");
            }
            return View(banner);
        }


        // GET: FrontPages/Delete/5
        public async Task<IActionResult> Banner_Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.FontPages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: FrontPages/Delete/5
        [HttpPost, ActionName("Banner_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Banner_DeleteConfirmed(int id)
        {
            var banner = await _context.FontPages.FindAsync(id);

            if (!string.Equals(banner.ImageName, "noimage.png"))
            {
                string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/layout/banner");
                string oldImagePath = Path.Combine(uploadsDir, banner.ImageName);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }



            _context.FontPages.Remove(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction("Banner_Index");
        }

        #endregion

        private bool FrontPageExists(int id)
        {
            return _context.FontPages.Any(e => e.Id == id);
        }
    }
}
