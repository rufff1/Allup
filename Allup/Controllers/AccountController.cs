using Allup.Model;
using Allup.ViewModels.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    public class AccountController : Controller
    {
        //rollari yaratmagcun
        private readonly RoleManager<IdentityRole> _roleManager;
        //userleri yaratmagcun
        private readonly UserManager<AppUser> _userManager;
        //login logout prosesleri ucun gonderdiyimiz datalar duzgundurse gedir session yaradir brauzerde.
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

      

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

              if (!ModelState.IsValid)
            {
                return View(registerVM);
            }


        


            return RedirectToAction("Login");
        }
    }
}
