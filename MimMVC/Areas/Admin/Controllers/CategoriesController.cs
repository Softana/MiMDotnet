using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;

namespace MimMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = MimMVC.WC.AdminRole)]
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext context;

		public CategoriesController(ApplicationDbContext context)
		{
			this.context = context;
		}

		//Get/admin/Categories
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Index()
		{
			return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
		}

		//Get/admin/Categories/create
		[Authorize(Roles = "Admin, LærerAdmin")]
		public IActionResult Create() => View();

		//Post/admin/Categories/Create
		[HttpPost]
		[Authorize(Roles = "Admin, LærerAdmin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			if (ModelState.IsValid)
			{
				category.Slug = category.Name.ToLower().Replace(" ", "-");
				category.Sorting = 100;

				var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Siden findes i forvejen .. ");
					return View(category);
				}

				context.Add(category);
				await context.SaveChangesAsync();

				TempData["Success"] = "En kategori er blevet tilføjet ..";

				return RedirectToAction("Index");
			}
			return View(category);
		}

		//Get/admin/Categories/edit/5
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Edit(int id)
		{
			Category category = await context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		//Post/admin/Categories/edit/5
		[HttpPost]
		[Authorize(Roles = "Admin, LærerAdmin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Category category)
		{
			if (ModelState.IsValid)
			{
				category.Slug = category.Name.ToLower().Replace(" ", "-");

				var slug = await context.Pages.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "The category already exists. ");
					return View(category);
				}

				context.Update(category);
				await context.SaveChangesAsync();

				TempData["Success"] = "The Page has been edited";

				return RedirectToAction("Edit", new { id });
			}
			return View(category);
		}


		//Get/admin/Categories/Delete/5
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Delete(int id)
		{
			Category category = await context.Categories.FindAsync(id);
			if (category == null)
			{
				TempData["Error"] = "The category does not exist!";
			}
			else
			{
				context.Categories.Remove(category);
				await context.SaveChangesAsync();
				TempData["Success"] = "The category has been deleted";
			}
			return RedirectToAction("Index");
		}

		//Post/admin/pages/categories/Reorder
		[HttpPost]
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Reorder(int[] id)
		{
			int count = 1;

			foreach (var categoryId in id)
			{
				Category category = await context.Categories.FindAsync(categoryId);
				category.Sorting = count;
				context.Update(category);
				await context.SaveChangesAsync();
				count++;
			}
			return Ok();
		}
	}
}
