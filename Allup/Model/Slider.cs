using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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

    }
}
