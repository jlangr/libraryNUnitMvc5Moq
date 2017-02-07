using NUnit.Framework;
using Library.Models;

namespace LibraryTest.Library.Models
{
    [TestFixture]
    public class PatronTest
    {
        const int Id = 101;
        const string Name = "Joe";
        const int HoldingId = 2;
        private Patron patron;

        [SetUp]
        public void Initialize()
        {
            patron = new Patron(Id, Name);
        }

        [Test]
        public void BalanceIsZeroOnCreation()
        {
            Assert.That(patron.Balance, Is.Zero);
        }

        [Test]
        public void FinesIncreaseBalance()
        {
            patron.Fine(0.10m);
            Assert.That(patron.Balance, Is.EqualTo(0.10m));
            patron.Fine(0.10m);
            Assert.That(patron.Balance, Is.EqualTo(0.20m));
        }

        [Test]
        public void RemitReducesBalance()
        {
            patron.Fine(1.10m);

            patron.Remit(0.20m);

            Assert.That(patron.Balance, Is.EqualTo(0.90m));
        }
    }
}
