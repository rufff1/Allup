using Allup.DAL;
using Allup.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{

    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _context.Categories.Include(c => c.Products).Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();

            return View(categories);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //HER HANSI CRUD VIEWSUNA DATA SET ELEMEK ISTESEK VIEWBAGNAN EDIRIK.
            //MEN CREATE CATEGORY VIEWSUNA CATEGORYIN DATASINI OTURDUM LAZIM OLANLARI ISTIFADE ETDIM.
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();

            return View();
        }




        //methodda olan IFormFile file property= IFormFile(formnan gelen file datasini tutmagcun) file(formun name propertisine verdiyimiz ad) 
        //metodun parametrinde seligesiz gorsenir categoriya modelinde yazdig IFormFile file ni.
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            //viewbag yuxarida create yazdigimizdi.bura yazdigki cunki burdan data yene getmelidi.
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }


            return View();
        }



    }
}
