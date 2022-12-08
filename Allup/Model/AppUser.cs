using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Model
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
