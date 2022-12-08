using Allup.DAL;
using Allup.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{

    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Category category)
        {
            //viewbag yuxarida create yazdigimizdi.bura yazdigki cunki burdan data yene getmelidi.
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _context.Categories.AnyAsync(c=> c.IsDeleted == false && c.Name.ToLower() == category.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name",$"This name {category.Name} already exists");
                return View(category);

            }

            if (category.IsMain)
            {
                if (category.File == null)
                {
                    ModelState.AddModelError("File","Fayl mecburidi");
                    return View(category);
                }

                if (category.File.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("File", "Faylin tipi image/jpeg olmalidir");
                    return View(category);
                }

                if ((category.File.Length / 1024)>20)
                {
                    ModelState.AddModelError("File", "Faylin olcusu maksimum 20 kb olmalidir");
                    return View(category);
                }

                string path = Path.Combine(_env.WebRootPath, "assets", "images");

              

                string FileName = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.AddHours(4).ToString("yyyyyMMddHHmmss") + "-" + category.File.FileName;
                string fullpath = Path.Combine(path, FileName);

                using (FileStream fileStream = new FileStream(fullpath, FileMode.Create))
                {
                    await category.File.CopyToAsync(fileStream);
                }

                category.ParentId = null;
                category.Image = FileName;




            }
            else
            {
                if (category.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Ust categoriya mutleg secilmelidir");
                    return View(category);
                }

                if (! await _context.Categories.AnyAsync(c=> c.IsDeleted==false && c.IsMain==true && c.Id == category.ParentId))
                {
                    ModelState.AddModelError("ParentId", "Duzgun ust categoriya secilmelidir");
                    return View(category);
                }

                category.Image = null;
            }


            category.Name = category.Name.Trim();
            category.IsDeleted = false;
            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            category.CreatedBy = "System";

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);

            if (category == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == true).ToListAsync();

            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            if (category.Id != id)
            {
                return BadRequest("Id bos ola bilmez");
            }


            if (await _context.Categories.AnyAsync(c => c.IsDeleted == false && c.Name.ToLower() == category.Name.ToLower().Trim() && c.Id == id))
            {
                ModelState.AddModelError("Name", $"This name {category.Name} already exists");
                return View(category);

            }

            Category existedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);

            if (existedCategory == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }





            if (category.IsMain)
            {
                if (existedCategory.Image == null && category.File == null)
                {
                    ModelState.AddModelError("File", "Fayl mecburidi");
                    return View(category);
                }

                if (category.File != null)
                {
                    if (category.File.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("File", "Faylin tipi image/jpeg olmalidir");
                        return View(category);
                    }

                    if ((category.File.Length / 1024) > 20)
                    {
                        ModelState.AddModelError("File", "Faylin olcusu maksimum 20 kb olmalidir");
                        return View(category);
                    }





                    //string path = @"C:\Users\ROG\Desktop\Allup\Allup\wwwroot\assets\images";

                    string path = Path.Combine(_env.WebRootPath, "assets", "images");

                    if (System.IO.File.Exists(Path.Combine(path,existedCategory.Image)))
                    {
                        System.IO.File.Delete(Path.Combine(path, existedCategory.Image));
                    }

                    string FileName = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.AddHours(4).ToString("yyyyyMMddHHmmss") + "-" + category.File.FileName;
                    string fullpath = Path.Combine(path, FileName);

                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Create))
                    {
                        await category.File.CopyToAsync(fileStream);
                    }

                    existedCategory.ParentId = null;
                    existedCategory.Image = FileName;
                }

            }
            else
            {
                if (category.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Ust categoriya mutleg secilmelidir");
                    return View(category);
                }

                if (!await _context.Categories.AnyAsync(c => c.IsDeleted == false && c.IsMain == true && c.ParentId == c.Id))
                {
                    ModelState.AddModelError("ParentId", "Duzgun ust categoriya secilmelidir");
                    return View(category);
                }

                existedCategory.Image = null;
                existedCategory.ParentId = category.ParentId;
            }

            existedCategory.IsMain = category.IsMain;
            existedCategory.Name = category.Name.Trim();
            existedCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            existedCategory.UpdatedBy = "System";

      
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");




        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Category category = await _context.Categories
                .Include(c=> c.Products)
                .Include(c=> c.Children)
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);

            if (category == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            if ((category.Products.Count() > 0 && category.Products !=null) || (category.Children.Count() > 0 && category.Children != null))
            {
                return RedirectToAction("Index");
            }

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow.AddHours(4);
            category.DeletedBy = "System";


            await _context.SaveChangesAsync();

            return RedirectToAction("index");
            
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int? id, Category category)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }



        //    if (id == null)
        //    {
        //        return BadRequest("Id bos ola bilmez");
        //    }

            

        //    if (deletedCategory == null)
        //    {
        //        return NotFound("Daxil edilen Id yalnisdir");
        //    }


        //    if (category.Id != id)
        //    {
        //        return BadRequest("Id bos ola bilmez");
        //    }

            
        //   deletedCategory.IsDeleted = true;
        //   deletedCategory.DeletedAt = DateTime.UtcNow.AddHours(4);
        //   deletedCategory.DeletedBy = "System";

           
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Category category = await _context.Categories
                .Include(c=> c.Products)
                .Include(c=> c.Children)
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.IsMain == true && c.Id == id);

            if (category == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

          
            return View(category);

        
        }


        


    }
}
