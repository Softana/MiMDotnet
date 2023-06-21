using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MimMVC.Models;

namespace MimMVC.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Page> Pages { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Video> Videos { get; set; }
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public DbSet<Invoice> Invoices { get; set; }  
		public DbSet<FrontPage> FontPages { get; set; }
        public DbSet<WorkShopIndmeldelse> WorkShopIndmeldelser { get; set; }
        public DbSet<WorkShop> Workshops { get; set; }
	}
}
	