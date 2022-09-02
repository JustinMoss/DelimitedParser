namespace DelimitedParser.Domain
{
    /// <summary>
    /// Defines an object that stores and retrieves objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of the objects</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Stores an object of type T.
        /// </summary>
        /// <param name="entity">The object to store</param>
        void Add(T entity);
        
        /// <summary>
        /// Retrieves all the objects of type T.
        /// </summary>
        /// <returns>An enumerable of objects of type T</returns>
        IEnumerable<T> GetAll();
    }
}
