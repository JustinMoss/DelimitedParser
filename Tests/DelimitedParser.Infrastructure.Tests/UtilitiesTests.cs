using FluentAssertions;
using System.ComponentModel;

namespace DelimitedParser.Infrastructure.Tests
{
    public class UtilitiesTests
    {
        [Theory]
        [DisplayName("GetDelimiter method correctly returns delimiter")]
        [InlineData(null, " ")]
        [InlineData("", " ")]
        [InlineData("some thing here", " ")]
        [InlineData("some|thing|here", "|")]
        [InlineData("some | thing | here", "|")]
        [InlineData("some,thing,here", ",")]
        [InlineData("some, thing, here", ",")]
        public void GetDelimiter_Tests(string input, string expectedDelimiter)
        {
            var actualDelimiter = Utilities.GetDelimiter(input);

            actualDelimiter.Should().Be(expectedDelimiter);
        }
    }
}
