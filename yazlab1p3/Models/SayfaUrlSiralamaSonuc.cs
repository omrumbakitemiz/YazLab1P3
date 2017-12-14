using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yazlab1p3.Models
{
    public class SayfaUrlSiralamaSonuc
    {
        public string Url { get; set; }
        public int Siralama { get; set; }
        public List<string> AnahtarKelimeler { get; set; }
        public List<int> AnahtarKelimeGecmeSayisi { get; set; }
        public double Puan { get; set; }
    }
}
