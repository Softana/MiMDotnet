using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MimMVC.Models
{
    public class WorkShop
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Starter")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Du skal angive en start dato")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Slutter")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Du skal angive en start dato")]
        public DateTime FinishTime { get; set; }

        [Display(Name = "brugere")]
        public ICollection<WorkShopIndmeldelse> TilmeldteBrugere { get; set; }

        [Display(Name = "Gæster")]
        public int AntalGæster { get; set; }

        [Display(Name = "Total antal tilmeldte")]
        public int TotalTilmeldt { get; set; }

        [Display(Name = "File Navn")]
        public string FileName { get; set; }

        [NotMapped]
        [Display(Name = "Vedhæft Fil")]
        public IFormFile File { get; set; }

        [Display(Name = "Billede")]
        public string Image { get; set; }
    }
}