using FluentAssertions;

namespace DelimitedParser.Infrastructure.Tests
{
    public class PersonParserTests
    {
        [Fact(DisplayName = "Parse method with delimiter correctly creates a person record")]
        public void Parse_HappyPath1()
        {
            var personString = "last | first | email | favorite | 9/1/2022";

            var parser = new PersonParser();

            var person = parser.Parse(personString, "|");

            person.LastName.Should().Be("last");
            person.FirstName.Should().Be("first");
            person.Email.Should().Be("email");
            person.FavoriteColor.Should().Be("favorite");
            person.DateOfBirth.ToShortDateString().Should().Be("9/1/2022");
        }

        [Fact(DisplayName = "Parse method without delimiter correctly creates a person record")]
        public void Parse_HappyPath2()
        {
            var personString = "last first email favorite 9/1/2022";

            var parser = new PersonParser();

            var person = parser.Parse(personString);

            person.LastName.Should().Be("last");
            person.FirstName.Should().Be("first");
            person.Email.Should().Be("email");
            person.FavoriteColor.Should().Be("favorite");
            person.DateOfBirth.ToShortDateString().Should().Be("9/1/2022");
        }

        [Fact(DisplayName = "Parse method throw ArgumentException when using incorrect delimiter")]
        public void Parse_SadPath1()
        {
            var personString = "last first email favorite 9/1/2022";

            var parser = new PersonParser();

            var parse = () => parser.Parse(personString, "|");

            parse.Should().Throw<ArgumentException>().WithMessage("Supplied delimiter '|' was not found in the input data.");
        }

        [Fact(DisplayName = "Parse method throw ArgumentException when using invalid data")]
        public void Parse_SadPath2()
        {
            var personString = "last | first | email";

            var parser = new PersonParser();

            var parse = () => parser.Parse(personString, "|");

            parse.Should().Throw<ArgumentException>().WithMessage("Found 3 fields. A person requires 5 fields.");
        }
    }
}
