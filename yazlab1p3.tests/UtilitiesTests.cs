using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using yazlab1p3.Models;
using yazlab1p3.Util;

namespace yazlab1p3.tests
{
    public class UtilitiesTests
    {
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
        public void Score_ResultsTrue_ReturnTrue()
        {
            // var wordList = Utilities.SplitToWords(Utilities.RemoveHtmlTags(Utilities.GetHtmlSource("http://bilgisayar.kocaeli.edu.tr")));
            // var wordList = Utilities.SplitToWords(Utilities.RemoveHtmlTags(Utilities.GetHtmlSource("http://www.boun.edu.tr/Default.aspx?SectionID=127")));
            // var wordList = Utilities.SplitToWords(Utilities.RemoveHtmlTags(Utilities.GetHtmlSource("https://www.cmpe.boun.edu.tr/tr")));
            string[] keywords = {"bilgisayar", "proje", "numara","milli"};

            var searchResults1 = new List<SearchKeywordResult>
            {
                new SearchKeywordResult {Count = 4, Keyword = keywords[0]},
                new SearchKeywordResult {Count = 6, Keyword = keywords[1]},
                new SearchKeywordResult {Count = 5, Keyword = keywords[2]},
                new SearchKeywordResult {Count = 3, Keyword = keywords[3]}
            };

            var searchResults2 = new List<SearchKeywordResult>
            {
                new SearchKeywordResult {Count = 10, Keyword = keywords[0]},
                new SearchKeywordResult {Count = 0, Keyword = keywords[1]},
                new SearchKeywordResult {Count = 5, Keyword = keywords[2]},
                new SearchKeywordResult {Count = 2, Keyword = keywords[3]}
            };

            var searchResults3 = new List<SearchKeywordResult>
            {
                new SearchKeywordResult {Count = 5, Keyword = keywords[0]},
                new SearchKeywordResult {Count = 6, Keyword = keywords[1]},
                new SearchKeywordResult {Count = 8, Keyword = keywords[2]},
                new SearchKeywordResult {Count = 2, Keyword = keywords[3]}
            };

            var searchResults4 = new List<SearchKeywordResult>
            {
                new SearchKeywordResult {Count = 2, Keyword = keywords[0]},
                new SearchKeywordResult {Count = 6, Keyword = keywords[1]},
                new SearchKeywordResult {Count = 10, Keyword = keywords[2]},
                new SearchKeywordResult {Count = 0, Keyword = keywords[3]}
            };

            var webSiteList = new List<List<SearchKeywordResult>>();

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

            var result = Utilities.Score(webSiteList, numberListList);
            var actualResults = new List<double> { 0, 0, 2, 0 };

            Assert.Equal(result, actualResults);
        }
    }
}
