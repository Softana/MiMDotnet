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
	public class PagesController : Controller
	{
		private readonly ApplicationDbContext context;

		public PagesController(ApplicationDbContext context)
		{
			this.context = context;
		}

		//Get / admin / pages
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Index()
		{
			IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;

			List<Page> pagesList = await pages.ToListAsync();

			return View(pagesList);
		}

		//Get/admin/pages/details/5
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Details(int id)
		{
			Page page = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
			if (page == null)
			{
				return NotFound();
			}
			return View(page);
		}


		//Get/admin/pages/create
		[Authorize(Roles = "Admin, LærerAdmin")]
		public IActionResult Create() => View();

		//Post/admin/pages/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Create(Page page)
		{
			if (ModelState.IsValid)
			{
				page.Slug = page.Title.ToLower().Replace(" ", "-");
				page.Sorting = 100;

				var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Siden findes i forvejen! ");
					return View(page);
				}

				context.Add(page);
				await context.SaveChangesAsync();

				TempData["Success"] = "Siden er blevet tilføjet!";

				return RedirectToAction("Index");
			}

			return View(page);
		}

		//Get/admin/pages/edit/5
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Edit(int id)
		{
			Page page = await context.Pages.FindAsync(id);
			if (page == null)
			{
				return NotFound();
			}
			return View(page);
		}

		//Post/admin/pages/edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Edit(Page page)
		{
			if (ModelState.IsValid)
			{
				page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

				var slug = await context.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Siden findes i forvejen! ");
					return View(page);
				}

				context.Update(page);
				await context.SaveChangesAsync();

				TempData["Success"] = "Siden er blevet redigeret";

				return RedirectToAction("Edit", new { id = page.Id });
			}
			return View(page);
		}

		//Get/admin/pages/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			Page page = await context.Pages.FindAsync(id);
			if (page == null)
			{
				TempData["Error"] = "Siden findes ikke!";
			}
			else
			{
				context.Pages.Remove(page);
				await context.SaveChangesAsync();
				TempData["Success"] = "Siden er blevet slettet";
			}
			return RedirectToAction("Index");
		}

		//Post/admin/pages/Reorder
		[HttpPost]
		[Authorize(Roles = "Admin, LærerAdmin")]
		public async Task<IActionResult> Reorder(int[] id)
		{
			int count = 1;

			foreach (var pageId in id)
			{
				Page page = await context.Pages.FindAsync(pageId);
				page.Sorting = count;
				context.Update(page);
				await context.SaveChangesAsync();
				count++;
			}
			return Ok();
		}
	}
}
