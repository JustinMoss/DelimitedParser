using DelimitedParser.Domain;

namespace DelimitedParser.Infrastructure
{
    /// <summary>
    /// Defines an object that stores and retrieves Person objects.
    /// </summary>
    public class PersonRepository : IRepository<Person>
    {
        private readonly List<Person> _people = new List<Person>();

        /// <summary>
        /// Stores a Person object.
        /// </summary>
        /// <param name="entity">The Person to store</param>
        public void Add(Person entity) =>
            _people.Add(entity);

        /// <summary>
        /// Stores an enumerable of Person objects.
        /// </summary>
        /// <param name="enumerable">The enumerable of Person objects to store</param>
        public void AddBatch(IEnumerable<Person> enumerable) =>
            _people.AddRange(enumerable);

        /// <summary>
        /// Retrieves all Person objects.
        /// </summary>
        /// <returns>An enumerable of Person objects</returns>
        public IEnumerable<Person> GetAll()
            => _people;
    }
}