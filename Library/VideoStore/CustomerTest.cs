using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Env = System.Environment;

namespace Library.VideoStore
{
    [TestFixture]
    public class CustomerTest
    {
        private Customer customer;
        private const string TAB = "\t";

        [SetUp]
        public void Initialize()
        {
            customer = new Customer("Fred");
        }

        [Test]
        public void SingleNewReleaseStatement()
        {
            customer.Add(new Rental(new Movie("The Cell", Movie.NEW_RELEASE), 3));
            Assert.AreEqual(
                "Rental Record for Fred" + Env.NewLine +
                TAB + "The Cell" + TAB + "9.00" +  Env.NewLine +
                "You owed 9.00" + Env.NewLine + 
                "You earned 2 frequent renter points" + Env.NewLine,
                customer.Statement());
        }

        [Test]
        public void DualNewReleaseStatement()
        {
            customer.Add(new Rental(new Movie("The Cell", Movie.NEW_RELEASE), 3));
            customer.Add(new Rental(new Movie("The Tigger Movie", Movie.NEW_RELEASE), 3));
            Assert.AreEqual(
                "Rental Record for Fred" + Env.NewLine +
                TAB + "The Cell" + TAB + "9.00" + Env.NewLine +
                TAB + "The Tigger Movie" + TAB + "9.00" + Env.NewLine +
                "You owed 18.00" + Env.NewLine +
                "You earned 4 frequent renter points" + Env.NewLine,
                customer.Statement());
        }

        [Test]
        public void SingleChildrensStatement()
        {
            customer.Add(new Rental(new Movie("The Tigger Movie", Movie.CHILDRENS), 3));
            Assert.AreEqual(
                "Rental Record for Fred" + Env.NewLine +
                "\tThe Tigger Movie\t1.50" + Env.NewLine +
                "You owed 1.50" + Env.NewLine +
                "You earned 1 frequent renter points" + Env.NewLine,
                customer.Statement());
        }

        [Test]
        public void MultipleRegularStatement()
        {
            customer.Add(new Rental(new Movie("Plan 9 from Outer Space", Movie.REGULAR), 1));
            customer.Add(new Rental(new Movie("8 1/2", Movie.REGULAR), 2));
            customer.Add(new Rental(new Movie("Eraserhead", Movie.REGULAR), 3));

            Assert.AreEqual(
                "Rental Record for Fred" + Env.NewLine +
                "\tPlan 9 from Outer Space\t2.00" + Env.NewLine +
                "\t8 1/2\t2.00" + Env.NewLine +
                "\tEraserhead\t3.50" + Env.NewLine +
                "You owed 7.50" + Env.NewLine +
                "You earned 3 frequent renter points" + Env.NewLine,
                customer.Statement());
        }
    }
}
