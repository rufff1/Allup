using Allup.DAL;
using Allup.Model;
using Microsoft.AspNetCore.Hosting;
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
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env; 
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<Product> products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
                .Where(p=> p.IsDeleted == false)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {


            ViewBag.Brands = await _context.Brands.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => t.IsDeleted == false).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            ViewBag.Brands = await _context.Brands.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => t.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid)
            {
               
                return View(product);
            }

            if (!await _context.Categories.AnyAsync(c => c.IsDeleted == false && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId","gelen categoriya yalnisdir");
                return View(product);
            }

            if (product.BrandId == null)
            {
                ModelState.AddModelError("BrandId", "gelen brand yalnisdir");
                return View(product);
            }

            if (!await _context.Brands.AnyAsync(c => c.IsDeleted == false && c.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "gelen brand yalnisdir");
                return View(product);
            }

            if (!(product.Price > product.DiscountedPrice && product.DiscountedPrice > 0))
            {

                ModelState.AddModelError("DiscountedPrice", "endirimi duzgun qeyd edin");
                return View(product);
            }

            if (product.Price < 0)
            {

                ModelState.AddModelError("Price", "Qiymeti duzgun qeyd edin");
                return View(product);
            }

            //bos list tuturug secilen taglari elave etmeye asagida elave edirik
            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in product.TagIds)
            {
                if (product.TagIds.Where(t=> t==tagId).Count() > 1)
                {
                    ModelState.AddModelError("TagId", "bir tagdan yalniz bir defe secilmelidir");
                    return View(product);

                }

                  if (!await _context.Tags.AnyAsync(t => t.IsDeleted == false && t.Id == tagId))
                    {
                        ModelState.AddModelError("TagIds", "secilen tag yalnisdir");
                        return View(product);
                  }

                ProductTag productTag = new ProductTag
                {
                    CreatedAt = DateTime.UtcNow.AddHours(+4),
                    CreatedBy = "System",
                    IsDeleted = false,
                    TagId = tagId 
                    
                };

                //taglari bos liste add etdik
                productTags.Add(productTag);
            }

            if(product.MainImageFile == null)
             {
                ModelState.AddModelError("MainImageFile", "Fayl mecburidi");
                return View(product);
            }

            if (product.MainImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("MainImageFile", "Faylin tipi image/jpeg olmalidir");
                return View(product);
            }

            if ((product.MainImageFile.Length / 1024) > 20)
            {
                ModelState.AddModelError("MainImageFile", "Faylin olcusu maksimum 20 kb olmalidir");
                return View(product);
            }

            string path = Path.Combine(_env.WebRootPath, "assets", "images" , "product");



            string MainFileName = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.AddHours(4).ToString("yyyyyMMddHHmmss") + "-" + product.MainImageFile.FileName;
            string fullpath = Path.Combine(path, MainFileName);

            using (FileStream fileStream = new FileStream(fullpath, FileMode.Create))
            {
                await product.MainImageFile.CopyToAsync(fileStream);
            }


            if (product.HoverImageFile == null)
            {
                ModelState.AddModelError("HoverImageFile", "Fayl mecburidi");
                return View(product);
            }

            if (product.HoverImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("HoverImageFile", "Faylin tipi image/jpeg olmalidir");
                return View(product);
            }

            if ((product.HoverImageFile.Length / 1024) > 20)
            {
                ModelState.AddModelError("HoverImageFile", "Faylin olcusu maksimum 20 kb olmalidir");
                return View(product);
            }

          



            string HoverFileName = Guid.NewGuid().ToString() + "-" + DateTime.UtcNow.AddHours(4).ToString("yyyyyMMddHHmmss") + "-" + product.HoverImageFile.FileName;
            string fullpathHover = Path.Combine(path, HoverFileName);

            using (FileStream fileStream = new FileStream(fullpathHover, FileMode.Create))
            {
                await product.HoverImageFile.CopyToAsync(fileStream);
            }

            product.MainImage = MainFileName;
            product.HoverImage = HoverFileName;

            product.ProductTags = productTags;

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {

            ViewBag.Brands = await _context.Brands.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => t.IsDeleted == false).ToListAsync();

            if (id == null)
            {
                return BadRequest("Id null ola bilmez");

            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);

            if (product == null)
            {
                return NotFound("bele mehsul tapilmadi");
            }


            product.TagIds = await _context.ProductTags.Where(pt => pt.ProductId == id).Select(x => x.TagId).ToListAsync();
        

            return View(product);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product)
        {

            ViewBag.Brands = await _context.Brands.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => t.IsDeleted == false).ToListAsync();


            if (!ModelState.IsValid)
            {

                return View(product);
            }

            if (id == null)
            {
                return BadRequest("Id bos ola bilmez");
            }

            if (product.Id != id)
            {
                return BadRequest("Id bos ola bilmez");
            }

            Product existedProduct =await _context.Products.Include(p=> p.ProductTags).FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);

            if (existedProduct == null)
            {
                return NotFound("Daxil edilen Id yalnisdir");
            }

            _context.ProductTags.RemoveRange(existedProduct.ProductTags);

            //bos list tuturug secilen taglari elave etmeye asagida elave edirik
            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in product.TagIds)
            {
                if (product.TagIds.Where(t => t == tagId).Count() > 1)
                {
                    ModelState.AddModelError("TagId", "bir tagdan yalniz bir defe secilmelidir");
                    return View(product);

                }

                if (!await _context.Tags.AnyAsync(t => t.IsDeleted == false && t.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", "secilen tag yalnisdir");
                    return View(product);
                }

                ProductTag productTag = new ProductTag
                {
                    CreatedAt = DateTime.UtcNow.AddHours(+4),
                    CreatedBy = "System",
                    IsDeleted = false,
                    TagId = tagId

                };

                //taglari bos liste add etdik
                productTags.Add(productTag);
            }

            existedProduct.ProductTags = productTags;

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }
    }
}
