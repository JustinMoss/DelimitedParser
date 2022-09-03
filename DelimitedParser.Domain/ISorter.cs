namespace DelimitedParser.Domain
{
    /// <summary>
    /// Defines an object that sorts enumerables of objects of type T
    /// </summary>
    /// <typeparam name="T">The type of the objects to sort</typeparam>
    public interface ISorter<T>
    {
        /// <summary>
        /// Gets a enumerable of objects of type T sorted by a given field name.
        /// </summary>
        /// <param name="items">The enumerable to sort</param>
        /// <param name="fieldName">The field name for sorting</param>
        /// <returns></returns>
        IEnumerable<T> Sort(IEnumerable<T> items, string fieldName);
    }
}
