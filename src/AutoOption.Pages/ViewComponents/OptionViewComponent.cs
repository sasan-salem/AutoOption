using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AutoOption.Pages.ViewComponents
{
    public class OptionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IFormCollection Inputs)
        {
            List<OptionEntity> Options;

            if(Inputs == null)
                Options = OptionHelper.OptionEntities;
            else
            {
                Options = PageTools.ExtractOption(Inputs);
                OptionHelper.OptionEntities = Options;
            }

            return View(Options);
        }
    }
}
