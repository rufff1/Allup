using Allup.DAL;
using Allup.Model;
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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.Where(s => s.IsDeleted == false).ToListAsync();
            return View(sliders);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.File == null)
            {
                ModelState.AddModelError("File", "Fayl mecburidi");
                return View(slider);
            }

            if (slider.File.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("File", "Faylin tipi image/jpeg olmalidir");
                return View(slider);
            }

            if ((slider.File.Length / 1024) > 20)
            {
                ModelState.AddModelError("File", "Faylin olcusu maksimum 20 kb olmalidir");
                return View(slider);
            }

            string FileName = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.AddHours(4).ToString("yyyyyMMddHHmmss") + "-" + slider.File.FileName;
            string path = @"C:\Users\ROG\Desktop\Allup\Allup\wwwroot\assets\images" + slider.File.FileName;

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await slider.File.CopyToAsync(fileStream);
            }

            slider.Image = FileName;

            slider.IsDeleted = false;
            slider.CreatedAt = DateTime.UtcNow.AddHours(4);
            slider.CreatedBy = "System";

            await _context.Sliders.AddAsync(slider);
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

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s=> s.IsDeleted == false && s.Id == id);

            if (slider == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id , Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Slider existedslider = await _context.Sliders.FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);

            if (existedslider == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            if (slider.Id != id)
            {
                return BadRequest("Id bos ola bilmez");
            }


            if (slider.File == null)
            {
                ModelState.AddModelError("File", "Fayl mecburidi");
                return View(slider);
            }

            if (slider.File.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("File", "Faylin tipi image/jpeg olmalidir");
                return View(slider);
            }

            if ((slider.File.Length / 1024) > 20)
            {
                ModelState.AddModelError("File", "Faylin olcusu maksimum 20 kb olmalidir");
                return View(slider);
            }

            string FileName = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.AddHours(4).ToString("yyyyyMMddHHmmss") + "-" + slider.File.FileName;
            string path = @"C:\Users\ROG\Desktop\Allup\Allup\wwwroot\assets\images" + slider.File.FileName;

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await slider.File.CopyToAsync(fileStream);
            }

            existedslider.Image = FileName;

            existedslider.IsDeleted = false;
            existedslider.SubTitle = slider.SubTitle;
            existedslider.MainTitle = slider.MainTitle;
            existedslider.Description = slider.Description;
            existedslider.PageLink = slider.PageLink;
            existedslider.UpdatedAt = DateTime.UtcNow.AddHours(4);
            existedslider.UpdatedBy = "System";

          
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public  async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);

            if (slider == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,Slider slider)
        {
            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Slider deletedSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);

            if (deletedSlider == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            if (slider.Id != id)
            {
                return BadRequest("Id bos ola bilmez");
            }


            deletedSlider.IsDeleted = true;
            deletedSlider.DeletedAt = DateTime.UtcNow.AddHours(4);
            deletedSlider.DeletedBy = "System";

            _context.Sliders.Remove(deletedSlider);
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

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);

            if (slider == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            return View(slider);


        }
    }
}
