using DelimitedParser.Api.Apis;
using DelimitedParser.Domain;
using DelimitedParser.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IParser<Person>, PersonParser>();
builder.Services.AddSingleton<IParsingReader<Person>, PersonParsingReader>();
builder.Services.AddSingleton<IRepository<Person>, PersonRepository>();
builder.Services.AddSingleton<ISorter<Person>, PersonSorter>();

var app = builder.Build();

app.MapPersonApiRoutes();

app.Run();