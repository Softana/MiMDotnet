using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimMVC.Data;
using MimMVC.Models;
using MimMVC.Utility;

namespace MimMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMailService _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IMailService mailservice,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = mailservice;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<ApplicationUser> Teachers { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            //----------------- All -----------------//
            [Required]
            [StringLength(50, ErrorMessage = "{0} skal mindst være {2}.", MinimumLength = 3)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "{0} skal mindst være {2}.", MinimumLength = 3)]
            public string LastName { get; set; }

            [DataType(DataType.Date)]
            public DateTime DateBorn { get; set; }

            [Required]
            public int Age { get; set; }

            [Required]
            [StringLength(15, MinimumLength = 2)]
            public string PhoneNumber { get; set; }

            [StringLength(50, MinimumLength = 3)]
            public string Adress { get; set; }

            public int PostalCode { get; set; }
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string UserName { get; set; }

            //----------------- Student -----------------//
            public string ParentsFirstName { get; set; }

            public string ParentsLastName { get; set; }

            public string Instruction { get; set; }

            public string OneTeaching { get; set; }

            public string InteractWith { get; set; }

            public string NoYPlayed { get; set; }

            public string TaughtBy { get; set; }
           
            public string Comments { get; set; }

            //----------- Permits User -----------------//

            public bool PermitsWebside { get; set; }

            public bool PermitsSocial { get; set; }

            public bool PermitsAdvertising { get; set; }



            //----------------- Teacher -----------------//
            public string TeacherShotDescription { get; set; }

            public string TeacherDescription { get; set; }

            public string Teach { get; set; }

            // ----------------- Image Upload -----------------//
            public string ImageName { get; set; }
            [NotMapped]
            public IFormFile ImageFile { get; set; }


            // ----------------- Password -----------------//
            [Required]
            [StringLength(100, ErrorMessage = "{0} Skal mindst være {2}.", MinimumLength = 2)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "De to koder er ikke ens.")]
            public string ConfirmPassword { get; set; }       
    }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["Produkter"] = await _context.Products.ToListAsync();

            Teachers = (await _userManager.GetUsersInRoleAsync(WC.TeacherRole)).Concat((await _userManager.GetUsersInRoleAsync(WC.AdminTeacherRole)));
            
            ViewData["Lærer"] = await _userManager.GetUsersInRoleAsync("Lærer");

			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    //----------- All -----------------//
                    UserName = Input.UserName,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    DateBorn = Input.DateBorn,
                    Age = Input.Age,
                    PhoneNumber = Input.PhoneNumber,
                    Email = Input.Email,
                    Adress = Input.Adress,
                    PostalCode = Input.PostalCode,

                    //----------- Student -----------------//
                    ParentsFirstName = Input.ParentsFirstName,
                    ParentsLastName = Input.ParentsLastName,
                    Instruction = Input.Instruction,
                    OneTeaching = Input.OneTeaching,
                    InteractWith = Input.InteractWith,
                    NoYPlayed = Input.NoYPlayed,
                    TaughtBy = Input.TaughtBy,
                    Comments = Input.Comments,

                    //----------- Permits User -----------------//

                    
                    PermitsWebside = Input.PermitsWebside,
                    PermitsSocial = Input.PermitsSocial,
                    PermitsAdvertising = Input.PermitsAdvertising,


                    //----------- Teacher -----------------//
                    TeacherShotDescription = Input.TeacherShotDescription,
                    TeacherDescription = Input.TeacherDescription,
                    Teach = Input.Teach,

                    //----------- Image Upload -----------------//
                    ImageName = Input.ImageName,
                    ImageFile = Input.ImageFile
                };

                //----- Profile Images --------
                string imageName = "noimage.png";
                if (user.ImageFile != null)
                {
                    string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "img/profileImages");
                    imageName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await user.ImageFile.CopyToAsync(fs);
                    fs.Close();
                }
                //--------------
                user.ImageName = imageName;
                
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    
                    _logger.LogInformation("En bruger oprettede en ny brugerprofil ");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var pathLon = _hostEnvironment.WebRootPath + "/Templates/IndmeldEmail.html";
                    var html = System.IO.File.ReadAllText(pathLon);
                    html = html.Replace("{{BrugernavnLon}}", user.FullName);
                    html = html.Replace("{{InstruLon}}", user.Instruction);
                    html = html.Replace("{{StudentAge}}", user.Age.ToString());
                    html = html.Replace("{{Experience}}", user.NoYPlayed);
                    html = html.Replace("{{StutentEmail}}", user.Email);
                    html = html.Replace("{{StutentPhone}}", user.PhoneNumber);
                    await _emailSender.SendEmailOnlyBody("info@musikimalling.dk", "MiM Ny indmeldelse", html);

                    var pathLonBekreft = _hostEnvironment.WebRootPath + "/Templates/BekreftEmail.html";
                    var htmlBekreft = System.IO.File.ReadAllText(pathLonBekreft);
                    htmlBekreft = htmlBekreft.Replace("{{BrugerBekreft}}", user.FullName);
                    await _emailSender.SendEmailOnlyBody(user.Email, "Velkommen til Musik i Malling!", htmlBekreft);
                    
                    if (_signInManager.IsSignedIn(User))
                    {
                        if (user.TaughtBy is null)
                        {
                            await _userManager.AddToRoleAsync(user, WC.StudentRole);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, WC.AktivStudentRole);
                        }
                        return LocalRedirect(returnUrl);
                    }
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        if (user.TaughtBy is null)
                        {
                            await _userManager.AddToRoleAsync(user, WC.StudentRole);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, WC.AktivStudentRole);
                        }
                        
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.TaughtBy is null)
                        {
                            await _userManager.AddToRoleAsync(user, WC.StudentRole);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, WC.AktivStudentRole);
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
           
            return Page();
        }
    }
}