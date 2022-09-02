namespace DelimitedParser.Domain
{
    /// <summary>
    /// Defines an object that parses files or text readers into an enumerable of objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of the objects being created</typeparam>
    public interface IParsingReader<T>
    {
        /// <summary>
        /// Parses a text reader into an enumerable of objects of type T, using the given delimiter.
        /// </summary>
        /// <param name="reader">The name of the text reader to parse</param>
        /// <param name="delimiter">The delimiter used for parsing</param>
        /// <returns>An enumerable of objects of type T</returns>
        IEnumerable<T> Parse(TextReader reader, string delimiter);
    }
}
