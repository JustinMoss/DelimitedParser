namespace DelimitedParser.Api.Models
{
    /// <summary>
    /// Defines an object that represents the http request to add Person records.
    /// </summary>
    /// <param name="Record">The data line for parsing</param>
    /// <param name="Delimiter">The delimiter to use when parsing</param>
    public record PersonRecordRequest (string Record, string Delimiter);
}
