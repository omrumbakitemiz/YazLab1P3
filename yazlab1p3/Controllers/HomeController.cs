using System;
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

        //[HttpGet]
        //public IActionResult SayfaUrlSiralama(UrlKeyword urlKeyword)
        //{
        //    return View(urlKeyword);
        //}

        [HttpPost]
        public IActionResult SayfaUrlSiralama(UrlKeyword urlKeyword)
        {
            // List<string> urlList = model.Select(p => p.Url).ToList();

            List<SayfaUrlSiralamaSonuc> model = new List<SayfaUrlSiralamaSonuc>();

            //SayfaUrlSiralamaSonuc temp = new SayfaUrlSiralamaSonuc();
            //temp.Url = "http://bilgisayar.kocaeli.edu.tr";
            //temp.AnahtarKelimeler = new List<string>
            //{
            //    "bilgisayar",
            //    "proje"
            //};

            //SayfaUrlSiralamaSonuc temp2 = new SayfaUrlSiralamaSonuc();
            //temp2.Url = "https://www.ce.yildiz.edu.tr/";
            //SayfaUrlSiralamaSonuc temp3 = new SayfaUrlSiralamaSonuc();
            //temp3.Url = "https://ceng.metu.edu.tr/tr";

            //model.Add(temp);
            //model.Add(temp2);
            //model.Add(temp3);

            urlKeyword.Keywords = urlKeyword.Keywords[0].Replace("\r", "").Split('\n');
            urlKeyword.Urls = urlKeyword.Urls[0].Replace("\r", "").Split('\n');

            for (int i = 0; i < urlKeyword.Urls.Length; i++)
            {
                SayfaUrlSiralamaSonuc temp = new SayfaUrlSiralamaSonuc();
                temp.Url = urlKeyword.Urls[i];
                temp.AnahtarKelimeler = urlKeyword.Keywords.ToList();
                model.Add(temp);
            }

            for (int j = 0; j < urlKeyword.Keywords.Length; j++)
            {
                model[j].AnahtarKelimeler = urlKeyword.Keywords.ToList();
            }

            List<string> urlList = model.Select(p => p.Url).ToList();

            var sonuc = Utilities.SayfaUrlSiralama(urlList, model[0].AnahtarKelimeler);

            UrlKeyword result = new UrlKeyword();
            for (var i = 0; i < sonuc.Count; i++)
            {
                result.Keywords = sonuc[i].AnahtarKelimeler.ToArray();
            }

            result.Urls = sonuc.Select(p => p.Url).ToArray();
            result.Scores = sonuc.Select(p => Convert.ToInt32(p.Puan)).ToArray();
            
            return View(result);
        }

        public class UrlKeyword
        {
            public string[] Urls { get; set; }
            public string[] Keywords { get; set; }
            public int[] Scores { get; set; }
        }

        //[HttpGet]
        //public IActionResult UrlKeywordAl()
        //{
        //    return View();
        //}
        
        //[HttpPost]
        //public IActionResult UrlKeywordAl(UrlKeyword urlKeyword)
        //{
        //    return RedirectToPage("SayfaUrlSiralama", urlKeyword);
        //}


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
