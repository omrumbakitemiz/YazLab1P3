using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using yazlab1p3.Models;
using yazlab1p3.Util;

namespace yazlab1p3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string[] keywords = { "bilgisayar", "milli", "kocaeli" };
            var html = Utilities.GetHtmlSource("http://bilgisayar.kocaeli.edu.tr");
            var htmlWithoutTags = Utilities.RemoveHtmlTags(html);
            var wordList = Utilities.SplitToWords(htmlWithoutTags);
            var results = Utilities.SearchForKeywords(wordList, keywords);

            return View(results);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
