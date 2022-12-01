using Allup.DAL;
using Allup.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup.ViewModels.Basket;

namespace Allup.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id==null)
            {
                return BadRequest("id null ola bilmez");
            }

            //Product product = await _context.Products.FirstOrDefaultAsync(p => p.IsDeleted==false && p.Id==id);

            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id))
            {
                return NotFound("id yalnisdir");
            }



            string basket = HttpContext.Request.Cookies["basket"];


            List<BasketVM> products = null;

            if ( !string.IsNullOrWhiteSpace(basket) )
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                BasketVM basketVM = products.Find(p => p.Id == id);
                if (basketVM != null)
                {
                    basketVM.Count += 1;
                }
                else
                {
                    basketVM = new BasketVM
                    {
                        Id=(int)id,
                        Count =1

                    };
                    products.Add(basketVM);
                }
            }
            else
            {
                products = new List<BasketVM>();
                BasketVM basketVM = new BasketVM
                {
                    Id = (int)id,
                    Count = 1

                };
                products.Add(basketVM);
            }




            basket= JsonConvert.SerializeObject(products);
            HttpContext.Response.Cookies.Append("basket", basket);

            foreach (BasketVM basketVM in products)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.Id && p.IsDeleted == false);

                basketVM.Title = product.Title;
                basketVM.Image = product.MainImage;
                basketVM.ExTax = product.ExTax;
                basketVM.Price = product.DiscountedPrice > 0 ? product.DiscountedPrice : product.Price;
            }


            return PartialView("_BasketCardPArtial",products);
        }



        public async Task<IActionResult> GetFromBasket()
        {
           string pro =  HttpContext.Request.Cookies["basket"];
            List<BasketVM> products =JsonConvert.DeserializeObject<List<BasketVM>>(pro);

            return Json(products);

        }
    }
}
