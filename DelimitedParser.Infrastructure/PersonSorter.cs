using DelimitedParser.Domain;

namespace DelimitedParser.Infrastructure
{
    /// <summary>
    /// Defines an object that sorts enumerables of Person objects
    /// </summary>
    public class PersonSorter : ISorter<Person>
    {
        /// <summary>
        /// Gets a enumerable Person objects sorted by a given field name.
        /// </summary>
        /// <param name="items">The enumerable to sort</param>
        /// <param name="fieldName">The field name for sorting</param>
        /// <returns>A sorted enumerable of Person objects</returns>
        public IEnumerable<Person> Sort(IEnumerable<Person> items, string fieldName) =>
            // NOTE: This is a good place to discuss readability. Some may want a standard switch
            // and full body method to make it more readable. I prefer compression, but flexible to the team.
            fieldName switch
            {
                nameof(Person.LastName) => items.OrderByDescending(item => item.LastName),
                nameof(Person.DateOfBirth) => items.OrderBy(item => item.DateOfBirth),
                nameof(Person.FavoriteColor) => items.OrderBy(item => item.FavoriteColor).ThenBy(item => item.LastName),
                _ => throw new ArgumentOutOfRangeException(fieldName + " is not a valid fieldName value.", innerException: null),
            };
    }
}
