using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
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
        public static List<KeywordSearchResult> SearchForKeywords(List<string> wordList, string[] keywords)
        {
            List<KeywordSearchResult> resultList = new List<KeywordSearchResult>();

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
                
                KeywordSearchResult result = new KeywordSearchResult { Keyword = keyword, Count = count, Text = texts };
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

        /// <summary>
        /// Bu metod gelen arama sonuçlarını değerlendirerek bir skor listesi döndürür.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="numberListList"></param>
        /// <returns></returns>
        public static List<Score> Score(List<List<KeywordSearchResult>> results, List<List<double>> numberListList)
        {
            List<double> standartDeviationList = new List<double>();
            double webSiteCount = results.Count;

            var sums = results.Select(p => p.Sum(t => t.Count)).ToList();
            
            for (int i = 0; i < webSiteCount; i++)
            {
                standartDeviationList.Add(Math.Round(StandardDeviation(numberListList[i]), 4));
            }
            
            var countList = sums.Select(Convert.ToDouble).ToList();

            var countZScores = Zscore(countList);
            var countTScores = Tscore(countZScores);

            var standartDeviationZScores = Zscore(standartDeviationList);
            var standartDeviationTScores = Tscore(standartDeviationZScores);
            
            var scoreList = new List<Score>();

            for (int i = 0; i < webSiteCount; i++)
            {
                scoreList.Add(new Score
                {
                    LastScore = Math.Round(countTScores[i] - standartDeviationTScores[i], 4),
                    WebSiteId = i+1
                });
            }

            var orderedScoreList = scoreList.OrderByDescending(t => t.LastScore).ToList();
            
            return orderedScoreList;
        }

        public static List<string> GetSubUrls(string url)
        {
            List<string> subUrlList = new List<string>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = htmlWeb.Load(url);
            foreach (HtmlNode subUrl in document.DocumentNode.SelectNodes("//a[@href]"))
            {
                subUrlList.Add(subUrl.InnerText);
            }

            return new List<string>();
        }

        /// <summary>
        /// Bu metod verilen metnin içindeki Türkçe karakterleri(ç,ğ,ı,ü vb.) İngilizce karakterlerle değiştirir.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceTurkishCharacters(string text)
        {
            // abcçdefgğhıijklmnoöprsştuüvyz
            text = text.Replace('ç', 'c').Replace('ğ', 'g').
                Replace('ı', 'i').Replace('ö', 'o').
                Replace('ş', 's').Replace('ü', 'u');

            return text;
        }

        /// <summary>
        /// Bu metod verilen metnin içinedeki tüm karakterleri küçük harfe çevirir.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToLowercase(string text)
        {
            return text.ToLower();
        }
    }
}
