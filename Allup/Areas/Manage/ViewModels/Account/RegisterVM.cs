using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.ViewModels.Account
{
    public class RegisterVM
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Paswoord { get; set; }
        [DataType(DataType.Password)]
        //paswoordu yoxluyur inputdarda eynidi ya yox
        [Compare(nameof(Paswoord))]
        public string ConfirmPaswoord { get; set; }


    }
}
