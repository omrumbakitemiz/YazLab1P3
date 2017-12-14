using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yazlab1p3.Models
{
    public class SayfaUrlSiralama
    {
        public List<string> Urls { get; set; }
        public List<string> Keywords { get; set; }
        public List<Score> Scores { get; set; }
    }
}
