using DelimitedParser.Domain;

namespace DelimitedParser.Infrastructure
{
    /// <summary>
    /// Defines an object that parses text into objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object being created</typeparam>
    public class PersonParser : IParser<Person>
    {
        /// <summary>
        /// Parses the given input into a Person, with the given delimiter.
        /// </summary>
        /// <param name="input">The input to parse</param>
        /// <param name="delimiter">The delimiter used for parsing</param>
        /// <returns>An object of type T</returns>
        public Person Parse(string input, string? delimiter = null)
        {
            delimiter ??= Utilities.GetDelimiter(input);

            var fields = input.Split(delimiter);

            // NOTE: Depending on requirements, we could throw an exception if any of these fields were null.
            var lastName = fields[0]?.Trim();
            var firstName = fields[1]?.Trim();
            var email = fields[2]?.Trim();
            var favoriteColor = fields[3]?.Trim();
            var birthDate = DateTime.Parse(fields[4]);

            return new Person(lastName, firstName, email, favoriteColor, birthDate);
        }
    }
}
