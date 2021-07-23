using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoOption.Demo.Pages
{
    public class OptionsModel : PageModel
    {
        [BindProperty]
        public IFormCollection Inputs { get; set; }
    }
}