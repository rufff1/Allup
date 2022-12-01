using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Allup.Model
{
    public class Category :BaseEntity
    {

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsMain { get; set; }

        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
    }
}
