namespace DelimitedParser.Domain
{
    /// <summary>
    /// Defines a Person record.
    /// </summary>
    /// <param name="LastName">The person's last name</param>
    /// <param name="FirstName">The person's first name</param>
    /// <param name="Email">The person's email address</param>
    /// <param name="FavoriteColor">The person's favorite color</param>
    /// <param name="DateOfBirth">The person's birth date</param>
    public record Person(string LastName, string FirstName, string Email, string FavoriteColor, DateTime DateOfBirth);
}
