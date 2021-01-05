using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoOption.Demo.Pages
{
    public class OptionModel : PageModel
    {
        [BindProperty]
        public List<OptionEntity> Options { get; set; } = OptionHelper.OptionEntities;

        public void OnGet()
        {
            Options = OptionHelper.OptionEntities;
        }

        public void OnPost(IFormCollection Inputs)
        {
            Options = OptionHelper.OptionEntities;

            foreach (var item in Options)
            {
                item.Value = Inputs[item.Key];
            }
            OptionHelper.OptionEntities = Options;
        }
    }
}
