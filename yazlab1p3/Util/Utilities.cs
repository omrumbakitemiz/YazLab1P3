using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace yazlab1p3.Util
{
    public static partial class Utilities
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
            string temp_result = Regex.Replace(html, pattern, String.Empty);
            string result = Regex.Replace(temp_result, @"\t|\n|\r", String.Empty);
            
            return result;
        }

        public static List<SearchKeywordResult> SearchForKeywords(List<string> wordList, string[] keywords)
        {
            List<SearchKeywordResult> resultList = new List<SearchKeywordResult>();

            foreach (var keyword in keywords)
            {
                var results = from p in wordList
                             where p.Contains(keyword)
                             select p;

                string[] texts = new string[results.Count()];
                var count = results.Count();

                for (int i = 0; i < results.Count(); i++)
                {
                    texts[i] = results.ElementAt(i);
                }
                
                SearchKeywordResult result = new SearchKeywordResult { keyword = keyword, count = count, text = texts };
                resultList.Add(result);
            }
            
            return resultList;
        }
    }
}
