using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
//using MimMVC.Interfaces;
using MimMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MimMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using MimMVC.Utility;

namespace MimMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
		{
			_logger = logger;
            _context = context;
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var item = _context.FontPages.ToList();
            return View(item);
        }
        public IActionResult AboutEducation_Index()
        {
            var item = _context.Products.ToList();
            return View(item);
        }

        public IActionResult AboutUs_Index()
        {
            var item = _context.ApplicationUser.ToList();
            return View(item);
        }

        public IActionResult Practical_Index()
        {
            
            return View();
        }


        public async Task<IActionResult> Individual_AboutPage_Index(string id)
        {
            var item = await userManager.FindByIdAsync(id);
            return View(item);
        }

		[HttpGet]
		public IActionResult SendEmail()
        {

			return View();
        }



        public IActionResult Privacy()
		{
			return View();
		}

       
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
