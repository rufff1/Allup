using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allup.Model
{
    public class Category :BaseEntity
    {
        //annotations atributunda errorlar ingilis dilinde cixir istesek oz yazdigimiz cixsin bele yazirig mes:  [Required(ErrorMessage="Form bos buraxila bilmez")]
        //[Required]
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsMain { get; set; }

        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }




        // Manage panelde categoriya contrellerinin indexinde categroiyanin productunun sayini gotururuk ona gore yazdig..
        public IEnumerable<Product> Products { get; set; }


        //notmappet atributunu verdimki sonradan modelde yazmisam migration edende databasede qarisiglig olmasin.IFormFile file bura yazdig.
        //IFormFile file bi file secmeye imkan verir bir nece sekil secmeye imkan versek list tipinnen edirik => IEnumerable<IFormFile> Files.
        // public IEnumerable<IFormFile> Files { get; set; } cox secmey ucun yazilis.
        //bize bir eded sekil secmek lazimdir
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
