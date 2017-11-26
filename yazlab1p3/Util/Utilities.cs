using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using yazlab1p3.Models;

namespace yazlab1p3.Util
{
    public static class Utilities
    {
        /// <summary>
        /// Bu metod verilen url'in html kaynağını string şeklinde geri döndürür.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlSource(string url)
        {
            if (url == string.Empty || url == "")
            {
                return null;
            }
            string result;

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
            return result;
        }

        /// <summary>
        /// Bu metod verilen string ifadeyi kelimelere ayırır.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> SplitToWords(string text)
        {
            string[] tempResult = text?.Split(' ');

            return tempResult?.ToList();
        }

        /// <summary>
        /// Bu metod verilen string ifade içerisindeki html ifadelerini kaldırır.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtmlTags(string html)
        {
            string pattern = "<.*?>";
            string tempResult = Regex.Replace(html, pattern, string.Empty);
            string result = Regex.Replace(tempResult, @"\t|\n|\r", string.Empty);
            
            return result;
        }

        /// <summary>
        /// Bu metod verilen anahtar kelimeleri verilen kelime listesi içerisinde arar ve anahtar kelimelerin bulunma sayısını ve içerisinde bulunduğu kelimeyi geri döndürür.
        /// </summary>
        /// <param name="wordList"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
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
                
                SearchKeywordResult result = new SearchKeywordResult { Keyword = keyword, Count = count, Text = texts };
                resultList.Add(result);
            }
            
            return resultList;
        }

        /// <summary>
        /// Bu metod verilen sayı listesinin standart sapmasını bulur.
        /// </summary>
        /// <param name="numberList"></param>
        public static double StandardDeviation(List<double> numberList)
        {
            if (numberList == null)
            {
                return 0;
            }
            var average = numberList.Average();
            var squareRootList = new List<double>();

            foreach (var item in numberList)
            {
                squareRootList.Add(Math.Pow(item - average, 2));
            }

            var variance = squareRootList.Sum() / (numberList.Count - 1);

            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Bu metod verilen sayıların z puanlarını hesaplar.
        /// </summary>
        /// <param name="numberList"></param>
        /// <returns></returns>
        public static List<double> Zscore(List<double> numberList)
        {
            var average = numberList.Average();
            var standardDeviation = StandardDeviation(numberList);
            var zScoreList = numberList.Select(item => Math.Round((item - average) / standardDeviation, 4)).ToList();

            return zScoreList;
        }

        /// <summary>
        /// Bu metod verilen z puanlarını t puanlarına dönüştürür.
        /// </summary>
        /// <param name="zPointList"></param>
        /// <returns></returns>
        public static List<double> Tscore(List<double> zPointList)
        {
            var tPointList = zPointList.Select(item => Math.Round(10 * item + 50, 4)).ToList();
            
            return tPointList;
        }

        public static List<double> Score(List<SearchKeywordResult> result)
        {
            var numberList = result.Select(item => Convert.ToDouble(item.Count)).ToList();
            
            return Zscore(numberList);
        }
    }
}
