using Allup.DAL;
using Allup.Model;
using Allup.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>Index()
        {
            //List<Setting> settings = _context.Settings.ToList();
            //ViewBag.Settings = settings;

            //List<Slider> sliders =await _context.Sliders.Where(s => s.IsDeleted == false).ToListAsync();
            //List<Category> categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();
            

            HomeVM homeVM = new HomeVM
            {
                Sliders = await _context.Sliders.Where(s => s.IsDeleted == false).ToListAsync(),
                Categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync(),
                 NewArrival = await _context.Products.Where(p => p.IsDeleted == false && p.IsNewArrival == true).ToListAsync(),
                 BestSeller = await _context.Products.Where(p => p.IsDeleted == false && p.IsBestSeller == true).ToListAsync(),
                 Featured = await _context.Products.Where(p => p.IsDeleted == false && p.IsFeatured == true).ToListAsync()
            };

            return View(homeVM);
        }
    }
}
