using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.CodeAnalysis.Semantics;
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

            var result = tempResult?.ToList();

            result?.RemoveAll(p => p == string.Empty);
            result?.RemoveAll(p => p == "");

            return result;
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
        /// Bu metod verilen url'in önce html kaynağını alır, sonra bu kaynağın Html Tag'lerini kaldırır ve kelimelere ayırıp döndürür.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> GetWordList(string url)
        {
            var html = GetHtmlSource(url);
            var htmlWithoutTag = RemoveHtmlTags(html);
            var text = ReplaceTurkishCharacters(htmlWithoutTag);
            var wordList = SplitToWords(text.ToLower());

            return wordList;
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
        /// Bu metod verilen url'in alt urllerinin listesini döndürür.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> GetSubUrls(string url)
        {
            List<string> subUrlList = new List<string>();
            List<string> wrongSubUrlList = new List<string>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = htmlWeb.Load(url);

            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]") ?? Enumerable.Empty<HtmlNode>())
            {
                string href = link.Attributes["href"].Value;
                if (!string.IsNullOrEmpty(href))
                {
                    // javascript:void(0) içerenler atlanacak *
                    // '#' olanlar atlanacak *
                    // http:// veya https:// içeren ama 'domain.com' içermeyenler atlanacak
                    // '/' ile başlayanlar alınacak 
                    // http://domain.com içerenler alınacak 
                    // içeriğinde mailto olanlar atlanacak

                    string[] fileTypes =
                        {"pdf", "doc", "docx", "ppt", "pptx", "xls", "xlsx", "epub", "odt", "odp", "pds", "txt", "rft", "jpeg", "jpg", "png", "zip", "rar"};

                    if (href.Contains("javascript") || href.Contains("mailto") || href == "#" || fileTypes.Any(p => href.Contains(p)) || href.Contains("index"))
                    {
                        wrongSubUrlList.Add(href);
                    }
                    else if(href.Contains("http") && !href.Contains(url))
                    {
                        wrongSubUrlList.Add(href);
                    }
                    else
                    {
                        subUrlList.Add(href);
                    }
                }
            }

            subUrlList.Remove(url+"/");

            #region URL'i absolute URL'e çevirme
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < subUrlList.Count; i++)
            {
                if (!subUrlList[i].StartsWith("http") && !subUrlList[i].Contains(url))
                {
                    var newUrl = builder.Append(url).Append("/").Append(subUrlList[i]).ToString();
                    subUrlList[i] = newUrl;

                    builder.Clear();
                }
            } 
            #endregion
            
            return subUrlList;
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

        /// <summary>
        /// Bu metod verilen anahtar kelimeleri verilen kelime listesi içerisinde arar ve anahtar kelimelerin bulunma sayısını ve içerisinde bulunduğu kelimeyi geri döndürür.
        /// </summary>
        /// <param name="wordList"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static List<KeywordSearchResult> SearchForKeywords(List<string> wordList, string[] keywords)
        {
            List<KeywordSearchResult> resultList = new List<KeywordSearchResult>();

            List<string> loweredKeywords = new List<string>();
            foreach (var keyword in keywords)
            {
                var temp = keyword.ToLower();

                loweredKeywords.Add(ReplaceTurkishCharacters(temp));
            }

            foreach (var keyword in loweredKeywords)
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

                KeywordSearchResult result = new KeywordSearchResult { Keyword = keyword, Count = count };
                resultList.Add(result);
            }

            return resultList;
        }

        public static void SearchSubUrls(List<string> urlList)
        {
            List<List<KeywordSearchResult>> searchResultListList = new List<List<KeywordSearchResult>>();
            List<List<Score>> scoreListList = new List<List<Score>>();
            
            foreach (var url in urlList)
            {
                var subUrlList = GetSubUrls(url);

                foreach (var subUrl in subUrlList)
                {
                    var wordList = GetWordList(subUrl);
                    string[] keywords = {"bilgisayar", "mühendis"};

                    var result = SearchForKeywords(wordList, keywords);
                    searchResultListList.Add(result);
                }
                scoreListList.Add(Score(searchResultListList));
            }

            var data = scoreListList;
        }

        public static void GenerateWebSiteTree(string url)
        {
            
        }
        
        public static List<KeywordSearchResult> KeywordSearch(string url, string[] keywords)
        {
            var wordList = GetWordList(url);

            return SearchForKeywords(wordList, keywords);
        }

        /// <summary>
        /// Bu metod gelen arama sonuçlarını değerlendirerek bir skor listesi döndürür.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<Score> Score(List<List<KeywordSearchResult>> results)
        {
            List<List<double>> numberList = new List<List<double>>();

            foreach (var result in results)
            {
                List<double> tempList = new List<double>();

                foreach (var item in result)
                {
                    tempList.Add(item.Count);
                }

                numberList.Add(tempList);
            }


            List<double> standartDeviationList = new List<double>();
            double webSiteCount = results.Count;

            var sums = results.Select(p => p.Sum(t => t.Count)).ToList();

            for (int i = 0; i < webSiteCount; i++)
            {
                standartDeviationList.Add(Math.Round(StandardDeviation(numberList[i]), 4));
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
                    WebSiteId = i + 1
                });
            }

            var orderedScoreList = scoreList.OrderByDescending(t => t.LastScore).ToList();

            return orderedScoreList;
        }
        
        public static List<SayfaUrlSiralamaSonuc> SayfaUrlSiralama(List<string> urls, List<string> keywords)
        {
            List<SayfaUrlSiralamaSonuc> result = new List<SayfaUrlSiralamaSonuc>();
            
            SayfaUrlSiralamaSonuc sonuc = new SayfaUrlSiralamaSonuc();
            var tempResult = KeywordSearch(urls[0], keywords.ToArray());

            var temp = tempResult.Select(item => item.Count).ToList();
            sonuc.AnahtarKelimeGecmeSayisi = temp;

            sonuc.Url = urls[0];
            sonuc.AnahtarKelimeler = keywords;

            var tempKeywordResultList = new List<List<KeywordSearchResult>>();
            foreach (var urlTemp in urls)
            {
                tempKeywordResultList.Add(KeywordSearch(urlTemp, keywords.ToArray()));
            }

            var data = Score(tempKeywordResultList);

            for (var i = 0; i < data.Count; i++)
            {
                var score = data[i];
                var tempSonuc = new SayfaUrlSiralamaSonuc();
                tempSonuc.Puan = score.LastScore;
                tempSonuc.Url = urls[i];
                tempSonuc.AnahtarKelimeler = keywords;

                result.Add(tempSonuc);
            }

            var tempKeywordResultArray = tempKeywordResultList.ToArray();
            for (int i = 0; i < tempKeywordResultArray.Length; i++)
            {
                var tempResults = tempKeywordResultArray[i].ToList();
                var tempAnahtarKelimeSayisi = new List<int>();
                for (int j = 0; j < tempResult.Count; j++)
                {
                    var tempCount = tempResults[j].Count;
                    tempAnahtarKelimeSayisi.Add(tempCount);
                }

                result[i].AnahtarKelimeGecmeSayisi = tempAnahtarKelimeSayisi;
            }
            
            return result;
            
        }
    }
}
