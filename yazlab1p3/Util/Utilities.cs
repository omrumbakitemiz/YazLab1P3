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

        /// <summary>
        /// Bu metod gelen arama sonuçlarını değerlendirerek bir skor listesi döndürür.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="numberListList"></param>
        /// <returns></returns>
        public static List<double> Score(List<List<SearchKeywordResult>> results, List<List<double>> numberListList)
        {
            List<int> sums = new List<int>();
            List<double> standartDeviationList = new List<double>();

            double keywordCount = Convert.ToDouble(results[0].Count);
            double webSiteCount = results.Count;

            //for (int i = 0; i < keywordCount; i++)
            //{
            //    sums.Add(results.Sum(p => p[i].Count));
            //}
            
            //var averages = sums.Select(item => item / keywordCount).ToList();

            //List<double> scores = new List<double>();
            //for (int i = 0; i < webSiteCount; i++)
            //{
            //    scores.Add(0);
            //}

            //for (int i = 0; i < keywordCount; i++)
            //{
            //    for (int j = 0; j < webSiteCount; j++)
            //    {
            //        var result = results[j]; 
            //        var count = result[i].Count;

            //        if (count >= averages[i])
            //        {
            //            scores[j]++;
            //        }
            //        else
            //        {
            //            scores[j]--;
            //        }
            //    }
            //}

            //for (int i = 0; i < webSiteCount; i++)
            //{
            //    standartDeviationList.Add(Math.Round(StandardDeviation(numberListList[i]), 4));
            //}

            //List<Score> scoreList = new List<Score>();

            //for (int i = 0; i < scores.Count; i++)
            //{
            //    scoreList.Add(new Score
            //    {
            //        CountScore = scores[i],
            //        StandardDeviationScore = standartDeviationList[i]
            //    });
            //}

            //List<Score> scoreList = new List<Score>
            //{
            //    //new Score
            //    //{
            //    //    CountScore = 6,
            //    //    StandardDeviationScore = 2.5
            //    //},
            //    //new Score
            //    //{
            //    //    CountScore = 4,
            //    //    StandardDeviationScore = 1.5
            //    //},
            //    //new Score
            //    //{
            //    //    CountScore = 3,
            //    //    StandardDeviationScore = 6.7
            //    //},
            //    //new Score
            //    //{
            //    //    CountScore = 2,
            //    //    StandardDeviationScore = 3.5
            //    //}
            //};

            //var countScoreList = scoreList.Select(t => t.CountScore).ToList();
            //var standartDeviationScoreList = scoreList.Select(p => p.StandardDeviationScore).ToList();

            //var countScoreZScores = Zscore(countScoreList);
            //var countScoreTScores = Tscore(countScoreZScores);

            //var standartDeviationScoreZScores = Zscore(standartDeviationScoreList);
            //var standartDeviationScoreTScores = Tscore(standartDeviationScoreZScores);

            //double[] data1 = new double[4];
 
            //for (int i = 0; i < 4; i++)
            //{
            //    data1[i] = countScoreTScores[i] - standartDeviationScoreTScores[i];
            //}

            //var tempData = scoreList.OrderByDescending(p => p.CountScore).ToList();
            //var data = tempData.OrderBy(t => t.StandardDeviationScore).ToList();
            
            return scores;
        }
    }

    public class Score
    {
        public double CountScore;
        public double StandardDeviationScore;
    }
}
