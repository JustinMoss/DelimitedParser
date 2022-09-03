using DelimitedParser.Domain;
using FluentAssertions;
using System.ComponentModel;

namespace DelimitedParser.Infrastructure.Tests
{
    public class PersonSorterTests
    {
        [Fact]
        [DisplayName("Sort method correctly sorts with LastName fieldName")]
        public void Sort_HappyPath1()
        {
            var person1 = new Person("last1", "first2", "email3", "favorite4", new DateTime(2022, 9, 5));
            var person2 = new Person("last2", "first3", "email4", "favorite5", new DateTime(2022, 9, 1));
            var person3 = new Person("last3", "first4", "email5", "favorite1", new DateTime(2022, 9, 2));
            var person4 = new Person("last4", "first5", "email1", "favorite2", new DateTime(2022, 9, 3));
            var person5 = new Person("last5", "first1", "email2", "favorite3", new DateTime(2022, 9, 4));
            var people = new List<Person> { person3, person2, person5, person1, person4 };

            var sorter = new PersonSorter();

            var sorted = sorter.Sort(people, nameof(Person.LastName)).ToList();

            sorted[0].Should().BeEquivalentTo(person5);
            sorted[1].Should().BeEquivalentTo(person4);
            sorted[2].Should().BeEquivalentTo(person3);
            sorted[3].Should().BeEquivalentTo(person2);
            sorted[4].Should().BeEquivalentTo(person1);
        }

        [Fact]
        [DisplayName("Sort method correctly sorts with DateOfBirthField fieldName")]
        public void Sort_HappyPath2()
        {
            var person1 = new Person("last1", "first2", "email3", "favorite4", new DateTime(2022, 9, 5));
            var person2 = new Person("last2", "first3", "email4", "favorite5", new DateTime(2022, 9, 1));
            var person3 = new Person("last3", "first4", "email5", "favorite1", new DateTime(2022, 9, 2));
            var person4 = new Person("last4", "first5", "email1", "favorite2", new DateTime(2022, 9, 3));
            var person5 = new Person("last5", "first1", "email2", "favorite3", new DateTime(2022, 9, 4));
            var people = new List<Person> { person3, person2, person5, person1, person4 };

            var sorter = new PersonSorter();

            var sorted = sorter.Sort(people, nameof(Person.DateOfBirth)).ToList();

            sorted[0].Should().BeEquivalentTo(person2);
            sorted[1].Should().BeEquivalentTo(person3);
            sorted[2].Should().BeEquivalentTo(person4);
            sorted[3].Should().BeEquivalentTo(person5);
            sorted[4].Should().BeEquivalentTo(person1);
        }

        [Fact]
        [DisplayName("Sort method correctly sorts with FavoriteColor fieldName")]
        public void Sort_HappyPath3()
        {
            var person1 = new Person("last1", "first2", "email3", "red", new DateTime(2022, 9, 4));
            var person2 = new Person("last2", "first3", "email4", "blue", new DateTime(2022, 9, 5));
            var person3 = new Person("last3", "first4", "email5", "green", new DateTime(2022, 9, 6));
            var person4 = new Person("last4", "first5", "email6", "red", new DateTime(2022, 9, 7));
            var person5 = new Person("last5", "first6", "email7", "green", new DateTime(2022, 9, 8));
            var person6 = new Person("last6", "first7", "email8", "blue", new DateTime(2022, 9, 9));
            var person7 = new Person("last7", "first8", "email9", "green", new DateTime(2022, 9, 1));
            var person8 = new Person("last8", "first9", "email1", "red", new DateTime(2022, 9, 2));
            var person9 = new Person("last9", "first1", "email2", "blue", new DateTime(2022, 9, 3));
            var people = new List<Person> { person7, person2, person5, person9, person8, person6, person3, person4, person1 };

            var sorter = new PersonSorter();

            var sorted = sorter.Sort(people, nameof(Person.FavoriteColor)).ToList();

            sorted[0].Should().BeEquivalentTo(person9);
            sorted[1].Should().BeEquivalentTo(person6);
            sorted[2].Should().BeEquivalentTo(person2);
            sorted[3].Should().BeEquivalentTo(person7);
            sorted[4].Should().BeEquivalentTo(person5);
            sorted[5].Should().BeEquivalentTo(person3);
            sorted[6].Should().BeEquivalentTo(person8);
            sorted[7].Should().BeEquivalentTo(person4);
            sorted[8].Should().BeEquivalentTo(person1);
        }

        [Theory]
        [DisplayName("Sort method throws ArgumentOutOfRangeException on invalid display name")]
        [InlineData(nameof(Person.FirstName))]
        [InlineData(nameof(Person.Email))]
        [InlineData(null)]
        [InlineData("anything")]
        public void Sort_SadPaths(string fieldName)
        {
            var sorter = new PersonSorter();

            var action = () => sorter.Sort(new List<Person>(), fieldName).ToList();

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage(fieldName + " is not a valid fieldName value.");
        }
    }
}
