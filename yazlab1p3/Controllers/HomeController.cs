using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using yazlab1p3.Models;
using yazlab1p3.Util;

namespace yazlab1p3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //string[] keywords = { "bilgisayar", "milli", "kocaeli" };
            //var html = Utilities.GetHtmlSource("http://bilgisayar.kocaeli.edu.tr");
            //var htmlWithoutTags = Utilities.RemoveHtmlTags(html);
            //var wordList = Utilities.SplitToWords(htmlWithoutTags);
            //var results = Utilities.SearchForKeywords(wordList, keywords);

            return View();
        }

        [HttpGet]
        public IActionResult AnahtarKelimeSaydirma()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AnahtarKelimeSaydirma(AnahtarKelimeSaydirma model)
        {
            string[] keywords = {
                model.Keyword
            };

            var result = Utilities.KeywordSearch(model.Url, keywords);

            model.Count = result[0].Count;

            return View(model);
        }

        [HttpGet]
        public IActionResult SayfaUrlSiralama()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SayfaUrlSiralama(List<SayfaUrlSiralamaSonuc> model)
        {
            // List<string> urlList = model.Select(p => p.Url).ToList();

            model = new List<SayfaUrlSiralamaSonuc>();
            
            SayfaUrlSiralamaSonuc temp = new SayfaUrlSiralamaSonuc();
            temp.Url = "http://bilgisayar.kocaeli.edu.tr";
            temp.AnahtarKelimeler = new List<string>
            {
                "bilgisayar",
                "proje"
            };

            SayfaUrlSiralamaSonuc temp2 = new SayfaUrlSiralamaSonuc();
            temp2.Url = "https://www.ce.yildiz.edu.tr/";
            SayfaUrlSiralamaSonuc temp3 = new SayfaUrlSiralamaSonuc();
            temp3.Url = "https://ceng.metu.edu.tr/tr";

            model.Add(temp);
            model.Add(temp2);
            model.Add(temp3);

            List<string> urlList = model.Select(p => p.Url).ToList();

            var sonuc = Utilities.SayfaUrlSiralama(urlList, model[0].AnahtarKelimeler);

            ViewBag.sonuc = sonuc;
            
            return View(sonuc);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
