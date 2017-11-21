using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace yazlab1p3.Util
{
    public static class Utilities
    {
        public static string GetHtml(string url)
        {
            string result = String.Empty;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static List<string> SplitToWords(string text)
        {
            List<string> result = new List<string>();
            string[] temp_result = text.Split(' ');

            foreach (var word in temp_result)
            {
                result.Add(word);
            }

            return result;
        }

        public static string RemoveHtmlTags(string html)
        {
            string pattern = "<.*?>";
            string result = Regex.Replace(html, pattern, String.Empty);

            return result;
        }

        public static void SearchKeywords(string text, string[] keywords)
        {
            
        }
    }
}
