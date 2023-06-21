using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Models
{
	public class Page
	{
		public int Id { get; set; }

		[Required, MinLength(2, ErrorMessage = "Minimum længden er 2")]
		[Display(Name = "Titel")]
		public string Title { get; set; }

		public string Slug { get; set; }

		[Required, MinLength(4, ErrorMessage = "Minimum længden er 4")]
		[Display(Name = "Indhold")]
		public string Content { get; set; }

		public int Sorting { get; set; }
	}
}
