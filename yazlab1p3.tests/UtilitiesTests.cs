using System;
using System.Collections.Generic;
using Xunit;
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
            var result = Utilities.GetHtml(url);

            // Assertion
            Assert.NotNull(result);
        }
        [Fact]
        public void GetHtml_ResultIsNull_ReturnTrue()
        {
            // Arrange
            string url = string.Empty;

            // Act
            var result = Utilities.GetHtml(url);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SplitToWords_ResultNotContainsBlankSpace_ReturnTrue()
        {
            string textWithHtml = Utilities.GetHtml("http://bilgisayar.kocaeli.edu.tr");
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
            string text = null;

            var result = Utilities.SplitToWords(text);

            Assert.Null(result);
        }

        [Fact]
        public void SearchForKeywords_ResultCount_ReturnTrue()
        {
            var text = Utilities.RemoveHtmlTags("<br>bilgisayar <h2>bilgisayar milli adli immino</h2> güvenlik güvenlik <div>computer deneme 12421</div> <h1>bilgisayar</h1>");
            var wordList = Utilities.SplitToWords(text);
            string[] keywords = { "bilgisayar", "milli", "güvenlik", "test" };

            var result = Utilities.SearchForKeywords(wordList, keywords);
            var resultCount = 0;

            foreach (var item in result)
            {
                resultCount += item.count;
            }

            Assert.Equal(6, resultCount);
        }

        [Fact]
        public void SearchForKeywords_ResultIsEmptyWhenInputIsEmpty_ReturnTrue()
        {
            var wordList = Utilities.SplitToWords(string.Empty);
            string[] keywords = { "bilgisayar", "milli", "güvenlik", "test" };

            var result = Utilities.SearchForKeywords(wordList, keywords);

            Assert.True(result[0].count == 0);
        }

        [Fact]
        public void RemoveHtmlTags_ResultNotContainHtmlTag_ReturnTrue()
        {
            var text = Utilities.RemoveHtmlTags("<html><br>bilgisayar <h2>bilgisayar milli adli immino</h2> güvenlik güvenlik <div>computer deneme 12421</div> <h1>bilgisayar</h1></html>");
            var result = Utilities.RemoveHtmlTags(text);

            Assert.DoesNotContain("<html>", result);
            Assert.DoesNotContain("<h1>", result);
            Assert.DoesNotContain("<div>", result);
            Assert.DoesNotContain("<a>", result);
        }
    }
}
