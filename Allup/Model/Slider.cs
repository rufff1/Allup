using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allup.Model
{
    public class Slider : BaseEntity
    {

        [StringLength(1000)]
        public string  SubTitle { get; set; }

        [StringLength(1000)]
        public string MainTitle { get; set; }

        [StringLength(2000)]
        public string  Description { get; set; }

        [StringLength(1000)]
        public string Image { get; set; }

        [StringLength(1000)]
        public string PageLink { get; set; }






        //notmappet atributunu verdimki sonradan modelde yazmisam migration edende databasede qarisiglig olmasin.IFormFile file bura yazdig.
        //IFormFile file bi file secmeye imkan verir bir nece sekil secmeye imkan versek list tipinnen edirik => IEnumerable<IFormFile> Files.
        // public IEnumerable<IFormFile> Files { get; set; } cox secmey ucun yazilis.
        //bize bir eded sekil secmek lazimdir
        [NotMapped]
        public IFormFile File { get; set; }

    }
}
