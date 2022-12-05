using Allup.DAL;
using Allup.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{

    [Area("manage")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
             _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.Where(b=> b.IsDeleted==false).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (brand.Name == null)
            {
                ModelState.AddModelError("Name" , "Brand adi daxil edin");
                return View(brand);
                
            }

            brand.IsDeleted = false;
            brand.CreatedAt = DateTime.UtcNow.AddHours(4);
            brand.CreatedBy = "System";

            await _context.Brands.AddAsync(brand);
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

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);


            if (brand == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Brand existedbrand = await _context.Brands.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);


            if (existedbrand == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            if (brand.Id != id)
            {
                return BadRequest("Id bos ola bilmez");
            }

            if (brand.Name == null)
            {
                ModelState.AddModelError("Name", "Brand adi daxil edin");
                return View(brand);

            }

            existedbrand.Name = brand.Name;
            existedbrand.UpdatedAt = DateTime.UtcNow.AddHours(4);
            existedbrand.UpdatedBy = "System";

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Brands = await _context.Brands.Where(c => c.IsDeleted == false).ToListAsync();
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Brand brand = await _context.Brands.FirstOrDefaultAsync(b=> b.IsDeleted == false && b.Id == id);
            if (brand == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }
           
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Brand brand)
        {
            ViewBag.Brands = await _context.Brands.Where(c => c.IsDeleted == false).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Brand deletedbrand = await _context.Brands.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (deletedbrand == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            if (brand.Id != id)
            {
                return BadRequest("Id bos ola bilmez");
            }

            deletedbrand.IsDeleted = true;
            deletedbrand.DeletedAt = DateTime.UtcNow.AddHours(4);
            deletedbrand.DeletedBy = "System";

             _context.Brands.Remove(deletedbrand);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }


            Brand brand = await _context.Brands.FirstOrDefaultAsync(b=> b.IsDeleted == false && b.Id == id);

            if (brand == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            return View(brand);
        }





    }


  
}
