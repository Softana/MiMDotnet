using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MimMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    //[Authorize(Roles = MimMVC.WC.AdminRole)]
    public class UsersController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _content;

        public UsersController(ApplicationDbContext content, UserManager<ApplicationUser>
            userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _content = content;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        //// GET /admin/Users/Index
        [Authorize(Roles = "Admin, Lærer, LærerAdmin, Kasserer, Leder, Orlov")]
        public IActionResult Index()
        {           

            var userList = _content.ApplicationUser.ToList();
            var userRole = _content.UserRoles.ToList();
            var roles = _content.Roles.ToList();
            foreach (var user in userList)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "Bruger";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
            }

            userList = userList.OrderByDescending(x => x.Role).ToList();

            return View(userList);
        }

        [Authorize(Roles = "Admin, Lærer, LærerAdmin, Kasserer, Leder, Orlov")]
        public IActionResult SortUsers(string Role) {

            var userList = _content.ApplicationUser.ToList();
            var userRole = _content.UserRoles.ToList();
            var roles = _content.Roles.ToList();

            foreach (var user in userList)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "Bruger";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
            }

            userList = userList.OrderByDescending(x => x.Role == Role).ThenByDescending( x => x.Role).ThenByDescending(x => x.FullName).ToList();

            return View("Index", userList);
        }


        // GET /admin/Users/details/5
        //[HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }


        //// GET /admin/Users/Edit/5
        [Authorize(Roles = "Admin, LærerAdmin, Leder, Orlov")]
        public IActionResult Edit(string userId)
        {
            var objFromDb = _content.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            var userRole = _content.UserRoles.ToList();
            var roles = _content.Roles.ToList();
            var role = userRole.FirstOrDefault(u => u.UserId == objFromDb.Id);
            if (role != null)
            {
                objFromDb.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
            }
            objFromDb.RoleList = _content.Roles.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            });
            return View(objFromDb);
        }

        [Authorize(Roles = "Admin, LærerAdmin, Leder, Orlov")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var objFromDb = _content.ApplicationUser.FirstOrDefault(u => u.Id == user.Id);
                if (objFromDb == null)
                {
                    return NotFound();
                }
                var userRole = _content.UserRoles.FirstOrDefault(u => u.UserId == objFromDb.Id);
                if (userRole != null)
                {
                    var previousRoleName = _content.Roles.Where(u => u.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                    //removing the old role
                    await _userManager.RemoveFromRoleAsync(objFromDb, previousRoleName);
                }

                // --- Edit Img ---
                if (user.ImageFile != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/profileimages");

                    if (objFromDb.ImageName != "noimage.png" && objFromDb.ImageName != null)
                    {
                        string oldImagePath = Path.Combine(uploadsDir, objFromDb.ImageName);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await user.ImageFile.CopyToAsync(fs);
                    fs.Close();
                    user.ImageName = imageName;
                    objFromDb.ImageName = user.ImageName;
                }

                objFromDb.FirstName = user.FirstName != null ? user.FirstName : objFromDb.FirstName;
                objFromDb.LastName = user.LastName != null ? user.LastName : objFromDb.LastName;
                objFromDb.PhoneNumber = user.PhoneNumber != null ? user.PhoneNumber : objFromDb.PhoneNumber;
                objFromDb.Adress = user.Adress != null ? user.Adress : objFromDb.Adress;
                objFromDb.PostalCode = user.PostalCode != null ? user.PostalCode : objFromDb.PostalCode;
                _content.Update(objFromDb);
                                
                await _userManager.AddToRoleAsync(objFromDb, _content.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);
                
                //objFromDb.UserName = user.UserName;
              

                _content.SaveChanges();
                TempData["Success"] = "Brugeren er blevet redigeret . .!";
                return RedirectToAction(nameof(Index));
            }

            user.RoleList = _content.Roles.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            });
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            var elever = await _content.Users.Where(x => x.TaughtBy == userId).ToListAsync();
            elever.ForEach(x => x.TaughtBy = null);
            _content.Users.UpdateRange(elever);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                if (!user.ImageName.Equals("noimage.png"))
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/profileImages");
                    string oldImagePath = Path.Combine(uploadsDir, user.ImageName);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }
    }
}
