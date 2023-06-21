using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;

namespace MimMVC.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            this._hostEnvironment = webHostEnvironment;
        }

        [Display(Name = "Brugernavn")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<ApplicationUser> AssignedStudents { get; set; }
        public List<ApplicationUser> StudentsWithNoTeacher { get; set; }

        public class InputModel
        {
            [Display(Name = "Fornavn")]
            [StringLength(50, MinimumLength = 3)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Efternavn")]
            [StringLength(50, MinimumLength = 3)]
            public string LastName { get; set; }

            [Display(Name = "Fødselsdato")]
            [DataType(DataType.Date)]
            public DateTime DateBorn { get; set; }

            
            [Display(Name = "Elevens alder")]
            public int Age { get; set; }

            [Display(Name = "Adresse")]
            [StringLength(50, MinimumLength = 3)]
            public string Adress { get; set; }

            [Display(Name = "Postnummer")]
            public int PostalCode { get; set; }

            [Display(Name = "Forældres fornavn")]
            public string ParentsFirstName { get; set; }

            [Display(Name = "Forældres efternavn")]
            public string ParentsLastName { get; set; }

            [Phone]
            [Display(Name = "Telefonnummer")]
            public string PhoneNumber { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            // ----------------- Teacher -----------------//
            [Display(Name = "Kort beskrivelse af dig")]
            public string TeacherShotDescription { get; set; }

            [Display(Name = "Uddybet beskrivelse af dig")]
            public string TeacherDescription { get; set; }

            [Display(Name = "Underviser i")]
            public string Teach { get; set; }


            // ----------------- Image Upload -----------------//
            [Display(Name = "Upload Name")]
            public string ImageName { get; set; }
            [NotMapped]
            [Display(Name = "Upload File")]
            public IFormFile ImageFile { get; set; }
        }

        private void LoadAsync(ApplicationUser user)
        {
            var userName = user.UserName;
            var phoneNumber = user.PhoneNumber;
            var firstName = user.FirstName;
            var lastName = user.LastName;

            var adress = user.Adress;
            var postalCode = user.PostalCode;

            var parentsFirstName = user.ParentsFirstName;
            var parentsLastName = user.ParentsLastName;

            var dateBorn = user.DateBorn;
            var age = user.Age;


            var email = user.Email;
            var imageName = user.ImageName;
            var imageFile = user.ImageFile;

            var teacherShotDescription = user.TeacherShotDescription;
            var teacherDescription = user.TeacherDescription;
            var teach = user.Teach;

            Username = userName;

            Input = new InputModel
            {
                FirstName = firstName,
                LastName = lastName,
                Adress = adress,
                PostalCode = postalCode,
                ParentsFirstName = parentsFirstName,
                ParentsLastName = parentsLastName,
                PhoneNumber = phoneNumber,

                DateBorn = dateBorn,
                Age = age,

                Email = user.Email,
                ImageName = user.ImageName,
                ImageFile = user.ImageFile,

                TeacherShotDescription = teacherShotDescription,
                TeacherDescription = teacherDescription,
                Teach = teach

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Produkter"] = await _context.Products.ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Det var ikke muligt at tilføje brugeren med ID '{_userManager.GetUserId(User)}'.");
            }
            if ( User.IsInRole(WC.TeacherRole) || User.IsInRole(WC.AdminTeacherRole))
            {
                var inactivestudents = await _userManager.GetUsersInRoleAsync(WC.StudentRole);
                var activestudents = await _userManager.GetUsersInRoleAsync(WC.AktivStudentRole);
                AssignedStudents = activestudents.Where(x => x.TaughtBy == user.Id).ToList();
                StudentsWithNoTeacher = inactivestudents.Concat(activestudents.Where(x => x.TaughtBy == null).ToList()).ToList();
            }
           
            LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Det var ikke muligt at tilføje brugeren med ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                LoadAsync(user);
                return Page();
            }

            var changeduser = user;

            changeduser.Adress = Input.Adress;
            changeduser.FirstName = Input.FirstName;
            changeduser.LastName = Input.LastName;
            changeduser.PostalCode = Input.PostalCode;
            changeduser.ParentsFirstName = Input.ParentsFirstName;
            changeduser.ParentsLastName = Input.ParentsLastName;
            changeduser.DateBorn = Input.DateBorn;
            changeduser.Age = Input.Age;


            //----------- Teacher -----------------//
            changeduser.TeacherShotDescription = Input.TeacherShotDescription;
            changeduser.TeacherDescription = Input.TeacherDescription;
            changeduser.Teach = Input.Teach;

            //----------- Image Upload -----------------//
            
            changeduser.ImageFile = Input.ImageFile;


            

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Uventet fejl, da du forsøgte at ændre dit telefonummer";
                    return RedirectToPage();
                }
            }

            if (changeduser.ImageFile != null)
            {
                string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/profileImages/");

                if (!string.Equals(user.ImageName, "noimage.png"))
                {
                    if (user.ImageName is not null)
                    {
                        string oldImagePath = Path.Combine(uploadsDir, user.ImageName);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }

                string imageName = Guid.NewGuid().ToString() + "_" + changeduser.ImageFile.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await user.ImageFile.CopyToAsync(fs);
                fs.Close();
                changeduser.ImageName = imageName;

                
            }
            await _userManager.UpdateAsync(changeduser);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Din profil er blevet ændret";
            return RedirectToPage();
        }

       
    }
}
