using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MimMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public int Age { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateBorn { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Adress { get; set; }
        public int PostalCode { get; set; }
        public int City { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public string Role { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> RoleList { get; set; }

        public string ParentsFirstName { get; set; }
        public string ParentsLastName { get; set; }
        public string ParentsFullName
        {
            get { return ParentsFirstName + " " + ParentsLastName; }
        }

        public string Instruction { get; set; }
        public string OneTeaching { get; set; }
        public string InteractWith { get; set; }        
        public string NoYPlayed { get; set; }
        public string TaughtBy { get; set; }        
        public string Comments { get; set; }
        public ICollection<WorkShopIndmeldelse> TilmeldteWorkshops { get; set; }

        public bool PermitsWebside { get; set; }
        public bool PermitsSocial { get; set; }
        public bool PermitsAdvertising { get; set; }
        [Display(Name = "Kort beskrivelse om dig")]
        public string TeacherShotDescription { get; set; }
        public string TeacherDescription { get; set; }
        public string Teach { get; set; }
        public bool HasLoggedIn { get; set; } = false;
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }    
}
