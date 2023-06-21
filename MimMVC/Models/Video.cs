using Microsoft.AspNetCore.Http;
using MimMVC.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Models
{
	public class Video
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string VideoLink { get; set; }

		
		[Display(Name = "File Navn")]
		public string FileName { get; set; }

		[NotMapped]
		[Display(Name = "Vedhæft Fil")]
		public IFormFile File { get; set; }
	}
}
