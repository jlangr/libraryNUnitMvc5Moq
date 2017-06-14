using NUnit.Framework;
using Library;
using System;

namespace LibraryTests.LibraryTest.Util
{
    class APortfolio
    {
        private Portfolio portfolio;

        public APortfolio()
        {
            Console.WriteLine("I am in the constructor");
        }

        [SetUp]
        public void Create()
        {
            Console.WriteLine("I am in the SetUp method");
            portfolio = new Portfolio();
        }

        [TearDown]
        public void DestroyStuff()
        {
            Console.WriteLine("close db connection");
        }

        [Test]
        public void IsEmptyWhenCreated()
        {
            Console.WriteLine("I am in IsEmptyWhenCreated");
            Assert.That(portfolio.IsEmpty, Is.True);
        }

        [Test]
        public void IsNoLongerEmptyAfterPurchase()
        {
            Console.WriteLine("I am in IsNoLongerEmptyAfterPurchase");
            portfolio.Purchase("ALL", 20);

            Assert.That(portfolio.IsEmpty, Is.False);
        }
    }
}
