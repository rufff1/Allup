using Allup.Model;
using Allup.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Interfaces
{
    public interface ILayoutService
    {
        Task<Dictionary<string, string>> GetSettingAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<BasketVM>> GetBasketVMsAsync();
    }
}
