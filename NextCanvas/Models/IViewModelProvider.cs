using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Models
{
    interface IViewModelProvider<out TModel> where TModel : new()
    {
        ViewModels.IViewModel<TModel> GetAssociatedViewModel();
    }
}
