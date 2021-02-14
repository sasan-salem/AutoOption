using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AutoOption.Pages
{
    public static class PageTools
    {
        public static List<OptionEntity> UpdateWithPostedInputs(IFormCollection Inputs)
        {
            var Options = OptionHelper.OptionEntities;

            foreach (var item in Options)
            {
                item.Value = Inputs[item.Key];
            }

            return Options;
        }
    }
}
