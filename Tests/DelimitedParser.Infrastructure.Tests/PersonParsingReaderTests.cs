using DelimitedParser.Domain;
using FluentAssertions;
using Moq;
using System.ComponentModel;

namespace DelimitedParser.Infrastructure.Tests
{
    public class PersonParsingReaderTests
    {
        [Fact()]
        [DisplayName("Parse method correctly reads and parses from a text reader")]
        public void Parse_HappyPath1()
        {
            var recordCount = 0;

            var mockParser = new Mock<IParser<Person>>();
            mockParser
                .Setup(m => m.Parse(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => new Person("last" + recordCount, "first", "email", "favorite", new DateTime(2022, 9, 1)));
            
            var mockReader = new Mock<TextReader>();
            mockReader
                .Setup(m => m.ReadLine())
                .Callback(() => recordCount++)
                .Returns(() => recordCount > 5 ? null : "anything");

            var parsingReader = new PersonParsingReader(mockParser.Object);
            
            var people = parsingReader.Parse(mockReader.Object, "|").ToList();

            people.Count().Should().Be(5);
            people[0].LastName.Should().Be("last1");
            people[1].LastName.Should().Be("last2");
            people[2].LastName.Should().Be("last3");
            people[3].LastName.Should().Be("last4");
            people[4].LastName.Should().Be("last5");
        }
    }
}
