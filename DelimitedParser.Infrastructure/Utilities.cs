namespace DelimitedParser.Infrastructure
{
    // NOTE: There are so many ways to handle utilities like this. How would your team?
    /// <summary>
    /// Contains a set of cross class utility methods.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Attempts to find an existent delimiter in an input string, returning space if none is found.
        /// </summary>
        /// <param name="input">The searched input string</param>
        /// <returns>A string delimiter</returns>
        public static string GetDelimiter(string input)
        {
            if (input is null)
                return " ";

            if (input.Contains('|'))
                return "|";
            else if (input.Contains(','))
                return ",";
            return " ";
        }
    }
}
