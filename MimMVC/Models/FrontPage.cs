using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MimMVC.Models
{
    public class FrontPage
    {
        public int Id { get; set; }


        // ------ PostNewContents ------
        [Display(Name = "Titel")]
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        

        // ------ Banner ------
        [Display(Name = "Indhold")]
        public string BannerContent { get; set; }
        [Display(Name = "Link")]
        public string BannerButtonLink { get; set; }
        [Display(Name = "Knap navn")]
        public string BannerButtonName { get; set; }
            
        [Display(Name = "Første Banner")]
        public bool FirstBanner { get; set; }


        // ------ Stiky Post ------
        [Display(Name = "Titel")]
        public string StikyTitle { get; set; }
        [Display(Name = "Indhold")]
        public string StikyContent { get; set; }

        [Display(Name = "Indhold 2")]
        public string StikyContent2 { get; set; }
        [Display(Name = "Titel 2")]
        public string StikyTitle2 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Dato for oprettelse")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La date est requise")]
        public DateTime Date { get; set; }


        // ------ Image Upload ------
        [Display(Name = "Upload Name")]
        public string ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
