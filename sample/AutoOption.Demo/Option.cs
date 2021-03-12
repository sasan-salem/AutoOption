using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoOption.Demo
{
    public class Options
    {
        [Display(Name = "SMS Panel")]
        public string SMSPanel { get; set; }

        [Display(Name = "Page Count")]
        public int PageCount { get; set; }

        [Display(Name = "Allow Comment")]
        public bool AllowComment { get; set; }

        [Display(Name = "Level")]
        public Level Level { get; set; }
    }

    public enum Level
    {
        Low,
        Medium,
        High,
        [Display(Name = "Very High")]
        VeryHigh = 10
    }
}
