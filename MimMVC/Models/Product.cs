using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MimMVC.Data;

namespace MimMVC.Models
{
	public class Product
	{
		public int Id { get; set; }

		public string Slug { get; set; }

		[Display(Name = "Titel")]
		public string Title { get; set; }

		[Display(Name = "Beskrivelse")]
		public string Description { get; set; }

		public string Author { get; set; }

		[Display(Name = "Enkelt lektion")]
		[Range(0, 5000)]
		public double SingleLekPrice { get; set; }

		[Display(Name = "Dobbelt lektion")]
		[Range(0, 5000)]
		public double DoubleLekPrice { get; set; }

		[Display(Name = "Gruppe")]
		[Range(0, 5000)]
		public double GroupLekPrice { get; set; }

		[Display(Name = "Varighed")]
		public TimeSpan LekTimeSpan { get; set; }

		[Display(Name = "Billede")]
		public string Image { get; set; }

		//[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Du skal vælge en kategorie")]
		public int CategoryId { get; set; }

		[Display(Name = "Kategori")]
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; }

		////[Required]
		//public int CoverTypeId { get; set; }
		//[ForeignKey("CoverTypeId")]
		//public CoverType CoverType { get; set; }
	}
}
