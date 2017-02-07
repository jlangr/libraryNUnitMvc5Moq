using System;
using NUnit.Framework;
using Library.VideoStore;

namespace LibraryTests.Library.VideoStore
{
    [TestFixture]
    public class CustomerTest
    {
        private Customer customer;
        private const string Tab = "\t";

        [SetUp]
        public void Initialize()
        {
            customer = new Customer("Fred");
        }

        [Test]
        public void SingleNewReleaseStatement()
        {
            customer.Add(new Rental(new Movie("The Cell", Movie.NewRelease), 3));
            Assert.That(
                "Rental Record for Fred" + Environment.NewLine +
                Tab + "The Cell" + Tab + "9.00" +  Environment.NewLine +
                "You owed 9.00" + Environment.NewLine + 
                "You earned 2 frequent renter points" + Environment.NewLine,
                Is.EqualTo(customer.Statement()));
        }

        [Test]
        public void DualNewReleaseStatement()
        {
            customer.Add(new Rental(new Movie("The Cell", Movie.NewRelease), 3));
            customer.Add(new Rental(new Movie("The Tigger Movie", Movie.NewRelease), 3));
            Assert.That(
                "Rental Record for Fred" + Environment.NewLine +
                Tab + "The Cell" + Tab + "9.00" + Environment.NewLine +
                Tab + "The Tigger Movie" + Tab + "9.00" + Environment.NewLine +
                "You owed 18.00" + Environment.NewLine +
                "You earned 4 frequent renter points" + Environment.NewLine,
                Is.EqualTo(customer.Statement()));
        }

        [Test]
        public void SingleChildrensStatement()
        {
            customer.Add(new Rental(new Movie("The Tigger Movie", Movie.Childrens), 3));
            Assert.That(
                "Rental Record for Fred" + Environment.NewLine +
                "\tThe Tigger Movie\t1.50" + Environment.NewLine +
                "You owed 1.50" + Environment.NewLine +
                "You earned 1 frequent renter points" + Environment.NewLine,
                Is.EqualTo(customer.Statement()));
        }

        [Test]
        public void MultipleRegularStatement()
        {
            customer.Add(new Rental(new Movie("Plan 9 from Outer Space", Movie.Regular), 1));
            customer.Add(new Rental(new Movie("8 1/2", Movie.Regular), 2));
            customer.Add(new Rental(new Movie("Eraserhead", Movie.Regular), 3));

            Assert.That(
                "Rental Record for Fred" + Environment.NewLine +
                "\tPlan 9 from Outer Space\t2.00" + Environment.NewLine +
                "\t8 1/2\t2.00" + Environment.NewLine +
                "\tEraserhead\t3.50" + Environment.NewLine +
                "You owed 7.50" + Environment.NewLine +
                "You earned 3 frequent renter points" + Environment.NewLine,
                Is.EqualTo(customer.Statement()));
        }
    }
}
