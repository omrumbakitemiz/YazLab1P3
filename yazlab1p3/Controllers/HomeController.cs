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

        [HttpPost]
        public IActionResult SayfaUrlSiralama(UrlKeyword urlKeyword)
        {

            if (string.IsNullOrEmpty(urlKeyword.Urls[0]) ||  string.IsNullOrEmpty(urlKeyword.Keywords[0]))
            {
                return RedirectToAction("Error");
            }

            List<SayfaUrlSiralamaSonuc> model = new List<SayfaUrlSiralamaSonuc>();
            
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
            var data = sonuc.Select(p => p.AnahtarKelimeGecmeSayisi).ToArray();

            UrlKeyword result = new UrlKeyword();
            for (var i = 0; i < sonuc.Count; i++)
            {
                result.Keywords = sonuc[i].AnahtarKelimeler.ToArray();
            }

            result.Urls = sonuc.Select(p => p.Url).ToArray();
            result.Scores = sonuc.Select(p => Convert.ToInt32(p.Puan)).ToArray();
            
            return View(result);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    
}
