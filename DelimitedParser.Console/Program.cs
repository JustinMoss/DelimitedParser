using BetterConsoleTables;
using DelimitedParser.Console;
using DelimitedParser.Domain;
using DelimitedParser.Infrastructure;
using Microsoft.Extensions.Configuration;

var config = GetConfiguration();
var fileSettings = GetFilesConfig(config);

// NOTE: This is your composition root. In a larger, more complicated application, we'd use some sort of DI container.
var personRepo = new PersonRepository();
var personParser = new PersonParser();
var personParsingReader = new PersonParsingReader(personParser);
var personSorter = new PersonSorter();

Console.WriteLine("Reading data....");

PopulateRepository(fileSettings, personRepo, personParsingReader);

Console.WriteLine("Data Loaded.");
Console.WriteLine();

var fullPeopleList = personRepo.GetAll();
var colorSortedList = personSorter.Sort(fullPeopleList, nameof(Person.FavoriteColor));
var lastNameSortedList = personSorter.Sort(fullPeopleList, nameof(Person.LastName));
var birthSortedList = personSorter.Sort(fullPeopleList, nameof(Person.DateOfBirth));

OutputSortedData("Favorite Color, then Last Name", colorSortedList);
OutputSortedData("Birthdate", birthSortedList);
OutputSortedData("Last Name descending", lastNameSortedList);

Console.WriteLine("Press any button to close the application.");
Console.ReadLine();

IConfigurationRoot GetConfiguration()
{
    var configurationBuilder = new ConfigurationBuilder();
    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    return configurationBuilder.Build();
}

IEnumerable<DataFileSetting> GetFilesConfig(IConfigurationRoot config) => 
    config.GetSection("DataFiles")
        .GetChildren()
        .Select(file => new DataFileSetting(file["File"], file["Delimiter"]));

void PopulateRepository(IEnumerable<DataFileSetting> fileSettings, PersonRepository personRepo, PersonParsingReader personParsingReader)
{
    // Gather the data from each file
    foreach (var fileSetting in fileSettings)
    {
        using (var reader = new StreamReader(fileSetting.File))
        {
            var list = personParsingReader.Parse(reader, fileSetting.Delimiter);
            personRepo.AddBatch(list);
        }
    }
}

void OutputSortedData(string displayHeader, IEnumerable<Person> list)
{
    Console.WriteLine($"Sorted by {displayHeader}:");
    
    var table = new Table(TableConfiguration.UnicodeAlt());
    table.AddColumns(Alignment.Left, Alignment.Left, "Last Name", "First Name", "Email", "Favorite Color", "Date of Birth");

    foreach (var person in list)
        table.AddRow(person.LastName, person.FirstName, person.Email, person.FavoriteColor, person.DateOfBirth.ToShortDateString());

    Console.Write(table.ToString());
    Console.WriteLine();
}