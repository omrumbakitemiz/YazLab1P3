using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yazlab1p3.Models
{
    public class UrlKeyword
    {
        public string[] Urls { get; set; }
        public string[] Keywords { get; set; }
        public int[] Scores { get; set; }
    }
}
