namespace DelimitedParser.Infrastructure
{
    // NOTE: There are so many ways to handle utilities like this. How would your team?
    public static class Utilities
    {
        public static string GetDelimiter(string input)
        {
            if (input.Contains('|'))
                return "|";
            else if (input.Contains(','))
                return ",";
            return " ";
        }
    }
}
