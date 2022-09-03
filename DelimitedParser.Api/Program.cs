using DelimitedParser.Api.Models;
using DelimitedParser.Domain;
using DelimitedParser.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IParser<Person>, PersonParser>();
builder.Services.AddSingleton<IParsingReader<Person>, PersonParsingReader>();
builder.Services.AddSingleton<IRepository<Person>, PersonRepository>();
builder.Services.AddSingleton<ISorter<Person>, PersonSorter>();

var app = builder.Build();

app.MapGet("/records/{sortField}", GetSortedPersonListResponse);
app.MapPost("/records", PostPersonRecords);

app.Run();

// NOTE: These could be moved off to another file, but given the overall size of
// this application, it feel like overkill.
IResult GetSortedPersonListResponse(string sortField, IRepository<Person> repository, ISorter<Person> sorter)
{
    try
    {
        string? fieldName =
           sortField switch
           {
               "color" => nameof(Person.FavoriteColor),
               "birthdate" => nameof(Person.DateOfBirth),
               "name" => nameof(Person.LastName),
               _ => null
           };

        if (fieldName is null)
            return Results.Problem(sortField + " is not a valid search parameter.", statusCode: StatusCodes.Status400BadRequest);

        var people = repository.GetAll();
        if (!people.Any())
            return Results.Problem("There were no people in the list to sort. Please try adding a person first.", statusCode: StatusCodes.Status500InternalServerError);

        var sortedPeople = sorter.Sort(people, fieldName);

        return Results.Ok(sortedPeople);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
}

IResult PostPersonRecords(PersonRecordRequest request, IRepository<Person> repository, IParsingReader<Person> reader)
{
    try
    {
        if (string.IsNullOrEmpty(request.Record))
            return Results.Problem("Required field 'Record' is missing", statusCode: StatusCodes.Status400BadRequest);

        var people = reader.Parse(new StringReader(request.Record), request.Delimiter);

        // NOTE: The requirements mention 'a single data line', which I interpret as a single string,
        // which in theory could allow more than one record. Plus, it makes it easier to bulk add data
        // for testing.
        repository.AddBatch(people);

        return Results.Ok("Successfully posted.");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
}