﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using yazlab1p3.Models;
using yazlab1p3.Util;

namespace yazlab1p3.tests
{
    public class UtilitiesTests
    {
        private readonly Random _random = new Random();

        [Fact]
        public void GetHtml_ResultIsNotNull_ReturnsTrue()
        {
            // Arrange
            string url = "http://www.kocaeli.edu.tr";

            // Act
            var result = Utilities.GetHtmlSource(url);

            // Assertion
            Assert.NotNull(result);
        }

        [Fact]
        public void GetHtml_ResultIsNull_ReturnTrue()
        {
            // Arrange
            string url = string.Empty;

            // Act
            var result = Utilities.GetHtmlSource(url);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SplitToWords_ResultNotContainsBlankSpace_ReturnTrue()
        {
            string textWithHtml = Utilities.GetHtmlSource("http://bilgisayar.kocaeli.edu.tr");
            string text = Utilities.RemoveHtmlTags(textWithHtml);
            var random = new Random();

            var result = Utilities.SplitToWords(text);

            for (int i = 0; i < (result.Count / 3); i++)
            {
                var randomIndex = random.Next(0, result.Count);
                var randomElement = result[randomIndex];
                Assert.DoesNotContain(" ", randomElement);
            }
        }

        [Fact]
        public void SplitToWord_ResultIsEmptyWhenInputIsEmpty_ReturnTrue()
        {
            string text = string.Empty;

            var result = Utilities.SplitToWords(text);

            Assert.True(result[0] == string.Empty && result.Count == 1);
        }

        [Fact]
        public void SplitToWord_ResultIsNullWhenInputIsNull_ReturnTrue()
        {
            var result = Utilities.SplitToWords(null);

            Assert.Null(result);
        }

        [Fact]
        public void SearchForKeywords_ResultCount_ReturnTrue()
        {
            var text = Utilities.RemoveHtmlTags(
                "<br>bilgisayar <h2>bilgisayar milli adli immino</h2> güvenlik güvenlik <div>computer deneme 12421</div> <h1>bilgisayar</h1>");
            var wordList = Utilities.SplitToWords(text);
            string[] keywords = {"bilgisayar", "milli", "güvenlik", "test"};

            var result = Utilities.SearchForKeywords(wordList, keywords);
            var resultCount = result.Sum(item => item.Count);

            Assert.Equal(6, resultCount);
        }

        [Fact]
        public void SearchForKeywordsSemantik()
        {
            string[] keywords = new string[] { "milli", "güvenlik" };
            Utilities.KeywordSearchSemantik("https://www.btk.gov.tr/", keywords);
        }

        [Fact]
        public void SearchForKeywords_ResultIsEmptyWhenInputIsEmpty_ReturnTrue()
        {
            var wordList = Utilities.SplitToWords(string.Empty);
            string[] keywords = {"bilgisayar", "milli", "güvenlik", "test"};

            var result = Utilities.SearchForKeywords(wordList, keywords);

            Assert.True(result[0].Count == 0);
        }

        [Fact]
        public void RemoveHtmlTags_ResultNotContainHtmlTag_ReturnTrue()
        {
            var text = Utilities.RemoveHtmlTags(
                "<html><br>bilgisayar <h2>bilgisayar milli adli immino</h2> güvenlik güvenlik <div>computer deneme 12421</div> <h1>bilgisayar</h1></html>");
            var result = Utilities.RemoveHtmlTags(text);

            Assert.DoesNotContain("<html>", result);
            Assert.DoesNotContain("<h1>", result);
            Assert.DoesNotContain("<div>", result);
            Assert.DoesNotContain("<a>", result);
        }

        [Fact]
        public void StandardDeviation_ResultIsTrue_ReturnTrue()
        {
            var numberList = new List<double> {10, 43, 12, 3, 8};
            var expectedResult = Math.Round(15.896540504147, MidpointRounding.ToEven);

            var actualResult = Math.Round(Utilities.StandardDeviation(numberList), MidpointRounding.ToEven);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void StandardDeviation_ResultIsZeroWhenInputNull_ReturnTrue()
        {
            var result = Utilities.StandardDeviation(null);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Zscore_ResultIsTrue_ReturnsTrue()
        {
            var expectedResultFor10 = -0.3271; // 10 değeri için z skoru
            var expectedResultFor43 = 1.7488; // 43 değeri için z skoru
            var expectedResultFor12 = -0.2013; // 12 değeri için z skoru
            var expectedResultFor3 = -0.7675; // 3 değeri için z skoru
            var expectedResultFor8 = -0.4529; // 8 değeri için z skoru
            var numberList = new List<double> {10, 43, 12, 3, 8};

            var actualResult = Utilities.Zscore(numberList);
            var zScoreFor10 = actualResult[0];
            var zScoreFor43 = actualResult[1];
            var zScoreFor12 = actualResult[2];
            var zScoreFor3 = actualResult[3];
            var zScoreFor8 = actualResult[4];

            Assert.Equal(expectedResultFor10, zScoreFor10);
            Assert.Equal(expectedResultFor43, zScoreFor43);
            Assert.Equal(expectedResultFor12, zScoreFor12);
            Assert.Equal(expectedResultFor3, zScoreFor3);
            Assert.Equal(expectedResultFor8, zScoreFor8);
        }

        [Fact]
        public void Tscore_ResultIsTrue_ReturnsTrue()
        {
            var expectedResultFor10 = 46.729; // 10 değeri için t skoru 46.729
            var expectedResultFor43 = 67.488; // 43 değeri için t skoru 67.488
            var expectedResultFor12 = 47.987; // 12 değeri için t skoru 47.987
            var expectedResultFor3 = 42.325; // 3 değeri için t skoru 42.325
            var expectedResultFor8 = 45.471; // 8 değeri için t skoru 45.471
            var expectedResultFor124 = 35.837; // 124 değeri için t skoru -1.4163
            var expectedResultFor332 = 61.016; // 332 değeri için t skoru 1.1016
            var expectedResultFor300 = 57.142; // 300 değeri için t skoru 0.7142
            var expectedResultFor250 = 51.089; // 250 değeri için t skoru 0.1089
            var expectedResultFor199 = 44.916; // 199 değeri için t skoru 0.5084
            var zPointList = Utilities.Zscore(new List<double> { 10, 43, 12, 3, 8 });
            var zPointList2 = Utilities.Zscore(new List<double> { 124, 332, 300, 250, 199 });

            var actualResult = Utilities.Tscore(zPointList);
            var actualResult2 = Utilities.Tscore(zPointList2);

            Assert.Equal(expectedResultFor10, actualResult[0]);
            Assert.Equal(expectedResultFor43, actualResult[1]);
            Assert.Equal(expectedResultFor12, actualResult[2]);
            Assert.Equal(expectedResultFor3, actualResult[3]);
            Assert.Equal(expectedResultFor8, actualResult[4]);

            Assert.Equal(expectedResultFor124, actualResult2[0]);
            Assert.Equal(expectedResultFor332, actualResult2[1]);
            Assert.Equal(expectedResultFor300, actualResult2[2]);
            Assert.Equal(expectedResultFor250, actualResult2[3]);
            Assert.Equal(expectedResultFor199, actualResult2[4]);
        }

        [Fact]
        public void Score_ResultIsTrue_ReturnTrue()
        {
            // var wordList = Utilities.SplitToWords(Utilities.RemoveHtmlTags(Utilities.GetHtmlSource("http://bilgisayar.kocaeli.edu.tr")));
            // var wordList = Utilities.SplitToWords(Utilities.RemoveHtmlTags(Utilities.GetHtmlSource("http://www.boun.edu.tr/Default.aspx?SectionID=127")));
            // var wordList = Utilities.SplitToWords(Utilities.RemoveHtmlTags(Utilities.GetHtmlSource("https://www.cmpe.boun.edu.tr/tr")));
            string[] keywords = {"bilgisayar", "proje", "numara","milli"};

            var searchResults1 = new List<KeywordSearchResult>
            {
                new KeywordSearchResult {Count = 4, Keyword = keywords[0], WebSiteId = 1},
                new KeywordSearchResult {Count = 6, Keyword = keywords[1], WebSiteId = 1},
                new KeywordSearchResult {Count = 5, Keyword = keywords[2], WebSiteId = 1},
                new KeywordSearchResult {Count = 3, Keyword = keywords[3], WebSiteId = 1}
            };

            var searchResults2 = new List<KeywordSearchResult>
            {
                new KeywordSearchResult {Count = 10, Keyword = keywords[0], WebSiteId = 2},
                new KeywordSearchResult {Count = 0, Keyword = keywords[1], WebSiteId = 2},
                new KeywordSearchResult {Count = 5, Keyword = keywords[2], WebSiteId = 2},
                new KeywordSearchResult {Count = 2, Keyword = keywords[3], WebSiteId = 2}
            };

            var searchResults3 = new List<KeywordSearchResult>
            {
                new KeywordSearchResult {Count = 5, Keyword = keywords[0], WebSiteId = 3},
                new KeywordSearchResult {Count = 6, Keyword = keywords[1], WebSiteId = 3},
                new KeywordSearchResult {Count = 8, Keyword = keywords[2], WebSiteId = 3},
                new KeywordSearchResult {Count = 2, Keyword = keywords[3], WebSiteId = 3}
            };

            var searchResults4 = new List<KeywordSearchResult>
            {
                new KeywordSearchResult {Count = 2, Keyword = keywords[0], WebSiteId = 4},
                new KeywordSearchResult {Count = 6, Keyword = keywords[1], WebSiteId = 4},
                new KeywordSearchResult {Count = 10, Keyword = keywords[2], WebSiteId = 4},
                new KeywordSearchResult {Count = 0, Keyword = keywords[3], WebSiteId = 4}
            };

            var webSiteList = new List<List<KeywordSearchResult>>();

            webSiteList.Add(searchResults1);
            webSiteList.Add(searchResults2);
            webSiteList.Add(searchResults3);
            webSiteList.Add(searchResults4);

            var numberList1 = new List<double> {4, 6, 5, 3};
            var numberList2 = new List<double> { 10, 0, 5, 2 };
            var numberList3 = new List<double> { 5, 6, 8, 2 };
            var numberList4 = new List<double> { 2, 6, 10, 0 };

            var numberListList = new List<List<double>>();

            numberListList.Add(numberList1);
            numberListList.Add(numberList2);
            numberListList.Add(numberList3);
            numberListList.Add(numberList4);

            var result = Utilities.Score(webSiteList);

            var actualResults = new List<Score>
            {
                new Score
                {
                    WebSiteId = 3
                },
                new Score
                {
                    WebSiteId = 1
                },
                new Score
                {
                    WebSiteId = 4
                },
                new Score
                {
                    WebSiteId = 2
                }
            };

            Assert.Equal(result[0].WebSiteId, actualResults[0].WebSiteId);
        }

        [Fact]
        public void GetSubUrls_ResultsIsListOfStrings_ReturnTrue()
        {
            //var d1 = Utilities.GetSubUrls("http://bilgisayar.kocaeli.edu.tr");
            //var d2 = Utilities.GetSubUrls("https://www.ce.yildiz.edu.tr/");
            //var d3 = Utilities.GetSubUrls("https://stackoverflow.com/");
            //var d4 = Utilities.GetSubUrls("https://www.microsoft.com/tr-tr");
            //var d5 = Utilities.GetSubUrls("https://www.vmware.com/");
            var d6 = Utilities.GetSubUrls("http://www.hurriyet.com.tr/");
            //var d7 = Utilities.GetSubUrls("https://www.youtube.com/");
            
            StreamWriter writer = new StreamWriter("suburls.txt");

            foreach (var item in d6)
            {
                writer.WriteLine(item);
            }

            writer.Close();

            Assert.DoesNotContain("javascript", d6);
            Assert.DoesNotContain("#", d6);
        }

        [Fact]
        public void ToLowercase_ResultNotContainCapital_ReturnTrue()
        {
            string[] testLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            string testText = "Bilgisayar bilgisayar milli MİLLİ güvenlik GüvenLik";

            var result = Utilities.ToLowercase(testText);
            var randomLetterIndex = _random.Next(0, testLetters.Length);

            for (int i = 0; i < 50; i++)
            {
                Assert.DoesNotContain(testLetters[randomLetterIndex], result);
                randomLetterIndex = _random.Next(0, testLetters.Length);
            }
        }

        [Fact]
        public void ReplaceTurkishCharacters_ResultNotContainTurkishCharacters_ReturnTrue()
        {
            string[] turkishCharacters = {"ç", "ğ", "ı", "ö", "ş", "ü"};
            string testText = "pijamalı hasta yağız şoföre çabucak güvendi";
            var randomCharacterIndex = _random.Next(0, turkishCharacters.Length);

            var result = Utilities.ReplaceTurkishCharacters(testText);

            for (int i = 0; i < 50; i++)
            {
                Assert.DoesNotContain(turkishCharacters[randomCharacterIndex], result);
                randomCharacterIndex = _random.Next(0, turkishCharacters.Length);
            }
        }

        [Fact]
        public void ReadDictionary_ResultsIsTrue_ReturnTrue()
        {
            string[] sampleInput =
                {"öğretmen;muallim", "aş;yemek", "ab;su", "aba;üstlük", "acaba;acep", "acıma;merhamet"};
            var path = "test.txt";

            StreamWriter writer = new StreamWriter(path);
            foreach (var item in sampleInput)
            {
                writer.WriteLine(item);
            }
            writer.Close();

            var dictionary = ReadDictionary.Read(path);

            Assert.Equal("muallim", dictionary.First(item => item.Key == "öğretmen").Value);
            Assert.Equal("yemek", dictionary.First(item => item.Key == "aş").Value);
            Assert.Equal("acep", dictionary.First(item => item.Key == "acaba").Value);
            Assert.Equal("üstlük", dictionary.First(item => item.Key == "aba").Value);
        }

        [Fact]
        public void SearchSubUrls()
        {
            List<string> urlList = new List<string>
            {
                "http://bilgisayar.kocaeli.edu.tr",
                "https://www.ce.yildiz.edu.tr/"
            };

            Utilities.SearchSubUrls(urlList);
        }

        [Fact]
        public void SubUrlTest()
        {
            // Utilities.GenerateWebSiteTree("http://bilgisayar.kocaeli.edu.tr");

            Tree tree = new Tree();

            // tree.AddData();

            tree.Test();
        }
    }
}
