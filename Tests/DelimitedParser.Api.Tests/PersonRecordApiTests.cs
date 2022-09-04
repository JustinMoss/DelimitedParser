using DelimitedParser.Api.Apis;
using DelimitedParser.Api.Models;
using DelimitedParser.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.ComponentModel;

namespace DelimitedParser.Api.Tests
{
    // NOTE: Unit testing minimal apis is very messing in .Net 6. This will improve in .Net 7 when the IResult
    // types are made public instead of internal. https://github.com/dotnet/aspnetcore/pull/40704
    public class PersonRecordApiTests
    {
        [Theory]
        [DisplayName("GetSortedPersonListResponse method returns ok result with sorted person list")]
        [InlineData("color")]
        [InlineData("birthdate")]
        [InlineData("name")]
        public async Task GetSortedPersonListResponse_HappyPaths(string sortField)
        {
            var httpContext = CreateFakeHttpContext();
            var repoPeople = new List<Person> { new Person("last1", "first1", "email1", "favorite1", new DateTime(2022, 9, 1)) };
            var sortedPeople = new List<Person> { new Person("last2", "first2", "email2", "favorite2", new DateTime(2022, 9, 2)) };

            var mockRepository = new Mock<IRepository<Person>>();
            mockRepository
                .Setup(m => m.GetAll())
                .Returns(repoPeople);

            var mockSorter = new Mock<ISorter<Person>>();
            mockSorter
                .Setup(m => m.Sort(It.IsAny<IEnumerable<Person>>(), It.IsAny<string>()))
                .Returns(sortedPeople);

            var result = PersonRecordApi.GetSortedPersonListResponse(sortField, mockRepository.Object, mockSorter.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            var actualPeople = JsonConvert.DeserializeObject<List<Person>>(body);

            actualPeople.Should().BeEquivalentTo(sortedPeople);
        }

        [Theory]
        [DisplayName("GetSortedPersonListResponse method returns error result with correct message when sortfield is invalid")]
        [InlineData("anything")]
        [InlineData(null)]
        public async Task GetSortedPersonListResponse_SadPath1s(string? sortField)
        {
            var httpContext = CreateFakeHttpContext();

            var mockRepository = new Mock<IRepository<Person>>();
            var mockSorter = new Mock<ISorter<Person>>();

            var result = PersonRecordApi.GetSortedPersonListResponse(sortField, mockRepository.Object, mockSorter.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            dynamic response = JsonConvert.DeserializeObject(body);

            ((string) response.detail).Should().Be($"'{sortField}' is not a valid search parameter.");
        }

        [Fact]
        [DisplayName("GetSortedPersonListResponse method returns error result with correct message when the person repo is empty")]
        public async Task GetSortedPersonListResponse_SadPath2()
        {
            var httpContext = CreateFakeHttpContext();

            var mockRepository = new Mock<IRepository<Person>>();
            mockRepository
                .Setup(m => m.GetAll())
                .Returns(new List<Person>());

            var mockSorter = new Mock<ISorter<Person>>();

            var result = PersonRecordApi.GetSortedPersonListResponse("color", mockRepository.Object, mockSorter.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            dynamic response = JsonConvert.DeserializeObject(body);

            ((string)response.detail).Should().Be("There were no people in the list to sort. Please try adding a person first.");
        }

        [Fact]
        [DisplayName("GetSortedPersonListResponse method returns error result with correct message when an exception occurs")]
        public async Task GetSortedPersonListResponse_SadPath3()
        {
            var httpContext = CreateFakeHttpContext();

            var mockRepository = new Mock<IRepository<Person>>();
            mockRepository
                .Setup(m => m.GetAll())
                .Throws(new Exception("This is some random exception"));

            var mockSorter = new Mock<ISorter<Person>>();

            var result = PersonRecordApi.GetSortedPersonListResponse("color", mockRepository.Object, mockSorter.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            dynamic response = JsonConvert.DeserializeObject(body);

            ((string)response.detail).Should().Be("This is some random exception");
        }

        [Fact]
        [DisplayName("PostPersonRecords method returns ok result and calls AddBatch repo method with parsed people")]
        public async Task PostPersonRecords_HappyPath()
        {
            var httpContext = CreateFakeHttpContext();

            IEnumerable<Person>? repoPeople = null;
            var parserPeople = new List<Person> { new Person("last1", "first1", "email1", "favorite1", new DateTime(2022, 9, 1)) };

            var mockRepository = new Mock<IRepository<Person>>();
            mockRepository
                .Setup(m => m.AddBatch(It.IsAny<IEnumerable<Person>>()))
                .Callback((IEnumerable<Person> people) => repoPeople = people);

            var mockParser = new Mock<IParsingReader<Person>>();
            mockParser
                .Setup(m => m.Parse(It.IsAny<TextReader>(), It.IsAny<string>()))
                .Returns(parserPeople);

            var result = PersonRecordApi.PostPersonRecords(new PersonRecordRequest("something", "something"), mockRepository.Object, mockParser.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

            body.Should().Be("\"Successfully posted.\"");

            repoPeople.Should().BeEquivalentTo(parserPeople);
        }

        [Fact]
        [DisplayName("PostPersonRecords method returns error result with correct error message when record is empty")]
        public async Task PostPersonRecords_SadPath1()
        {
            var httpContext = CreateFakeHttpContext();

            var mockRepository = new Mock<IRepository<Person>>();
            var mockParser = new Mock<IParsingReader<Person>>();

            var result = PersonRecordApi.PostPersonRecords(new PersonRecordRequest("", "something"), mockRepository.Object, mockParser.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            dynamic response = JsonConvert.DeserializeObject(body);

            ((string)response.detail).Should().Be("Required field 'Record' is missing");
        }

        [Fact]
        [DisplayName("PostPersonRecords method returns error result with correct message when exception is thrown")]
        public async Task PostPersonRecords_SadPath2()
        {
            var httpContext = CreateFakeHttpContext();

            var mockRepository = new Mock<IRepository<Person>>();
            mockRepository
                .Setup(m => m.AddBatch(It.IsAny<IEnumerable<Person>>()))
                .Throws(new Exception("This is a random error"));

            var mockParser = new Mock<IParsingReader<Person>>();

            var result = PersonRecordApi.PostPersonRecords(new PersonRecordRequest("anything", "something"), mockRepository.Object, mockParser.Object);
            await result.ExecuteAsync(httpContext);

            httpContext.Response.Body.Position = 0;
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            dynamic response = JsonConvert.DeserializeObject(body);

            ((string)response.detail).Should().Be("This is a random error");
        }

        private static HttpContext CreateFakeHttpContext() =>
            new DefaultHttpContext
            {
                RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
                Response = { Body = new MemoryStream(), },
            };
    }
}
