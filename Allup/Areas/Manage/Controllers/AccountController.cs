using Allup.Areas.Manage.ViewModels.Account;
using Allup.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{
    [Area("manage")]
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
     



        //ROLLARIMIZI YARATDIG
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole {Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
        //    return Ok();
        //}


        //SUPER ADMINI YARADIRIG    
        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    //superadmin yaratdig
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        Email = "superadmin@code.az",
        //        UserName = "SuperAdmin"
        //    };



        //    //birinci create edirik useri.
        //    await _userManager.CreateAsync(appUser);
        //    //paswoord add eledik super admine
        //    await _userManager.CreateAsync(appUser, "Superadmin123");
        //    //rolunu elave ediriik.
        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Ok();

        //}


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


            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

              //ERRORLARI YOXLUYUR MODELDEN
            //if (await _userManager.Users.AnyAsync(u => u.NormalizedUserName == registerVM.UserName.Trim().ToUpperInvariant()))
            //{
            //    ModelState.AddModelError("UserName", "Alreade Exists");
            //}

            //if (await _userManager.Users.AnyAsync(u => u.NormalizedEmail == registerVM.Email.Trim().ToUpperInvariant()))
            //{
            //    ModelState.AddModelError("UserName", "Alreade Exists");
            //}


                //DATABAZADAN GEDIR GETIRIR ERROLARI YOXLUYUR
            IdentityResult identityResult=await _userManager.CreateAsync(appUser,registerVM.Paswoord);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser, "Admin");




            //return RedirectToAction("Index","Dashboard",new { Areas = "manage" });

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }


             //gedir datalardan emaili axtarir yoxdusa error cixarir.
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("","Daxil etdiyiniz Email ve ya Sifre yalnisdir");
                return View(loginVM);
            }


            //parametrdeki true deyeri sturtapda pasworda verdiyimiz optionsa goredi 3 defe parolu sehf yigsa bloklanir.
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(appUser,loginVM.Paswoord,true);

            //paswoordu yoxladig
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Daxil etdiyiniz Email ve ya Sifre yalnisdir");
                return View(loginVM);
            }


            //brazuere datalari sessiondasaxlayir.remindme modelde yaratdig checkbox bool tipinnen viewda remember me checkboxuna gonderdik.
            //true paswordu 3 defe sehf yigsa blok. 
            await _signInManager.PasswordSignInAsync(appUser,loginVM.Paswoord,loginVM.RemindMe,true);

            return RedirectToAction("Index", "Dashboard", new { Areas = "manage" });
        }

    }
}
