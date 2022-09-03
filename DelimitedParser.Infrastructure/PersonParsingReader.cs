using DelimitedParser.Domain;

namespace DelimitedParser.Infrastructure
{
    /// <summary>
    /// Defines an object that parses files or text readers into an enumerable of Person object.
    /// </summary>
    public class PersonParsingReader : IParsingReader<Person>
    {
        private readonly IParser<Person> _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonParsingReader"/> class.
        /// </summary>
        /// <param name="parser">The parser used to create a person from a row</param>
        public PersonParsingReader(IParser<Person> parser)
        {
            _parser = parser;
        }

        /// <summary>
        /// Parses a text reader into an enumerable Person objects, using the given delimiter.
        /// </summary>
        /// <param name="reader">The name of the text reader to parse</param>
        /// <param name="delimiter">The delimiter used for parsing</param>
        /// <returns>An enumerable of Person objects</returns>
        public IEnumerable<Person> Parse(TextReader reader, string? delimiter)
        {
            var persons = new List<Person>();

            var row = reader.ReadLine();
            while (row != null)
            {
                delimiter ??= Utilities.GetDelimiter(row);

                var person = _parser.Parse(row, delimiter);
                persons.Add(person);

                row = reader.ReadLine();
            }

            return persons;
        }
    }
}
