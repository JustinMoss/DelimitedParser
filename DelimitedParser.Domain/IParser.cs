namespace DelimitedParser.Domain
{
    /// <summary>
    /// Defines an object that parses text into objects.
    /// </summary>
    /// <typeparam name="T">The type of the object</typeparam>
    public interface IParser<T>
    {
        /// <summary>
        /// Parses the given input into an object of type T, with the given delimiter.
        /// </summary>
        /// <param name="input">The input to parse</param>
        /// <param name="delimiter">The delimiter used for parsing</param>
        /// <returns>An object of type T</returns>
        T Parse(string input, string delimiter);
    }
}
