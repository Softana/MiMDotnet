using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = MimMVC.WC.AdminRole)]
    [Area("Admin")]
	public class RolesController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

        // GET /admin/roles
        [Authorize(Roles = "Admin, Kasserer, LærerAdmin, Lærer, LærerPaaOrlov")]
        public IActionResult Index() => View(_roleManager.Roles);

        // GET /admin/roles/create
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        // POST /admin/roles/create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([MinLength(2), Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "Rollen er blevet oprettet!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors) ModelState.AddModelError("", error.Description);
                }
            }

            ModelState.AddModelError("", "Minimum længden er 2");
            return View();
        }

        // GET /admin/roles/edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(id);

			List<ApplicationUser> members = new List<ApplicationUser>();
			List<ApplicationUser> nonMembers = new List<ApplicationUser>();

			foreach (ApplicationUser user in _userManager.Users.ToList())
			{
				var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
				list.Add(user);
			}

			return View(new RoleEdit
			{
				Role = role,
				Members = members,
				NonMembers = nonMembers
			});
		}

        // POST /admin/roles/edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEdit roleEdit)
        {
            IdentityResult result;

            foreach (string userId in roleEdit.AddIds ?? new string[] { })
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, roleEdit.RoleName);
            }

            foreach (string userId in roleEdit.DeleteIds ?? new string[] { })
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, roleEdit.RoleName);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }		
	}
}
