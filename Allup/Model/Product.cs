using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Allup.Model
{
    public class Product :BaseEntity
    {
        [StringLength(255)]
        [Display(Name = "Name")]
        public string Title { get; set; }
         [Column(TypeName = "money")]
        public double Price { get; set; }
        [Display(Name = "Endirimli Qiymet")]
        public double DiscountedPrice { get; set; }
        public double ExTax { get; set; }
        [StringLength(4)]
        public string Seria { get; set; }
        public int Code { get; set; }
        public int Count { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(255)]
        public string MainImage { get; set; }
        [StringLength(255)]
        public string HoverImage { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsFeatured { get; set; }
      
        
        
        public Nullable<int> BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }

        public IEnumerable<ProductTag> ProductTags { get; set; }

        [NotMapped]
        public IFormFile MainImageFile { get; set; }
        [NotMapped]
        public IFormFile HoverImageFile { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile> ProductImageFiles { get; set; }




        //many to many productnan tagin arasinda .bir nece tag idsi goturmekucun.
        [NotMapped]
        [MaxLength(3)]
        public IEnumerable<int> TagIds { get; set; }




    }
}
