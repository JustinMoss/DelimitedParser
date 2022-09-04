using DelimitedParser.Domain;
using FluentAssertions;

namespace DelimitedParser.Infrastructure.Tests
{
    public class PersonRepositoryTests
    {
        [Fact(DisplayName = "Add method correctly stores the Person")]
        public void Add_HappyPath()
        {
            var person = new Person("last", "first", "email", "favorite", new DateTime(2022, 9, 1));

            var repo = new PersonRepository();
            repo.Add(person);

            var people = repo.GetAll();

            people.Count().Should().Be(1);
            people.First().Should().BeEquivalentTo(person);
        }

        [Fact(DisplayName = "AddBatch method correctly stores the enumerable of Person objects")]
        public void AddBatch_HappyPath()
        {
            var personList = new List<Person>
            {
                new Person("last", "first", "email", "favorite", new DateTime(2022, 9, 1)),
                new Person("last2", "first2", "email2", "favorite2", new DateTime(2022, 9, 2)),
                new Person("last3", "first3", "email3", "favorite3", new DateTime(2022, 9, 3)),
                new Person("last4", "first4", "email4", "favorite4", new DateTime(2022, 9, 4)),
                new Person("last5", "first5", "email5", "favorite5", new DateTime(2022, 9, 5)),
            };

            var repo = new PersonRepository();
            repo.AddBatch(personList);

            var people = repo.GetAll();

            people.Count().Should().Be(5);
            people.Should().BeEquivalentTo(personList);
        }

        [Fact(DisplayName = "GetAll method correctly gets all the Person objects")]
        public void GetAll_HappyPath()
        {
            var personList = new List<Person>
            {
                new Person("last", "first", "email", "favorite", new DateTime(2022, 9, 1)),
                new Person("last2", "first2", "email2", "favorite2", new DateTime(2022, 9, 2)),
                new Person("last3", "first3", "email3", "favorite3", new DateTime(2022, 9, 3)),
                new Person("last4", "first4", "email4", "favorite4", new DateTime(2022, 9, 4)),
                new Person("last5", "first5", "email5", "favorite5", new DateTime(2022, 9, 5)),
            };

            var repo = new PersonRepository();

            foreach(var person in personList)
                repo.Add(person);

            var people = repo.GetAll();

            people.Count().Should().Be(5);
            people.Should().BeEquivalentTo(people);
        }
    }
}
