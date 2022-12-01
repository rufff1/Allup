using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(Dictionary<string,string> setting)
        {
            return View(await Task.FromResult(setting));
        }
    }
}
