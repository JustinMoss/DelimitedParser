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
        public IEnumerable<Person> Sort(IEnumerable<Person> items, string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Person.LastName):
                    return items.OrderByDescending(item => item.LastName);

                case nameof(Person.DateOfBirth):
                    return items.OrderBy(item => item.DateOfBirth);

                case nameof(Person.FavoriteColor):
                    return items.OrderBy(item => item.FavoriteColor).ThenByDescending(item => item.LastName);

                default:
                    throw new ArgumentOutOfRangeException(fieldName + " is not a valid fieldName value.", innerException: null);
            }
        }
    }
}
