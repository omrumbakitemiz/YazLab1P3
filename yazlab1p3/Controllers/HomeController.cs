using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using yazlab1p3.Models;
using Microsoft.AspNetCore.Http;
using yazlab1p3.Util;

namespace yazlab1p3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var html = Utilities.GetHtml("http://bilgisayar.kocaeli.edu.tr");
            var htmlWithoutTags = Utilities.RemoveHtmlTags(html);
            var words = Utilities.SplitToWords(htmlWithoutTags);
            

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
