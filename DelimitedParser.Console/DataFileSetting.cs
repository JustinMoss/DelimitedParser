namespace DelimitedParser.Console
{
    /// <summary>
    /// Defines DataFileSetting record. 
    /// </summary>
    /// <param name="FileName">The name of a data file</param>
    /// <param name="Delimiter">The delimiter of the data file</param>
    public record DataFileSetting(string File, string? Delimiter);
}
