using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimMVC.Data;
using MimMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Areas.Identity
{
    [Area("Identity")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public ManageController(ApplicationDbContext context, UserManager<ApplicationUser>
            userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> DeleteStudentFromTeacher(string id)
        {
            if (id is not null)
            {
                var student = await _userManager.FindByIdAsync(id);

                await _userManager.RemoveFromRoleAsync(student, WC.AktivStudentRole);
                await _userManager.AddToRoleAsync(student, WC.StudentRole);
                student.TaughtBy = null;
                await _context.SaveChangesAsync();
            }      
            return Redirect("/identity/account/manage/students");
        }

        public async Task<IActionResult> AddStudentToTeacher(string id)
        {
            if (id is not null)
            {
                var user = await _userManager.GetUserAsync(User);
                var student = await _userManager.FindByIdAsync(id);

                await _userManager.RemoveFromRoleAsync(student, WC.StudentRole);
                await _userManager.AddToRoleAsync(student, WC.AktivStudentRole);

                student.TaughtBy = user.Id;
                await _userManager.UpdateAsync(student);
            }

            return Redirect("/identity/account/manage/students");
        }
    }
}
