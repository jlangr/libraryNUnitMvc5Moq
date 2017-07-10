using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Util;

namespace LibraryTests.LibraryTest.Util
{
    class APortfolio
    {
        Portfolio portfolio;

        [SetUp]
        public void Create()
        {
            portfolio = new Portfolio();
        }

        [Test]
        public void IsEmptyBeforeAnyPurchases()
        {
            Assert.That(portfolio.IsEmpty, Is.True);
        }

        [Test]
        public void IsNoLongerEmptyAfterPurchase()
        {
            portfolio.Purchase("RELX", 10);

            Assert.That(portfolio.IsEmpty, Is.False);
        }

        [Test]
        public void UniqueSymbolCountIsZeroWhenNoPurchasesMade()
        {
            Assert.That(portfolio.UniqueSymbolCount, Is.Zero);
        }

        [Test]
        public void UniqueSymbolCountIncrementsOnPurchase()
        {
            portfolio.Purchase("IBM", 10);
            portfolio.Purchase("RELX", 20);

            Assert.That(portfolio.UniqueSymbolCount, Is.EqualTo(2));
        }

        [Test]
        public void UniqueSymbolCountDoesntIncrementOnPurchaseSameSymbol()
        {
            portfolio.Purchase("IBM", 10);
            portfolio.Purchase("IBM", 15);
            portfolio.Purchase("RELX", 20);

            Assert.That(portfolio.UniqueSymbolCount, Is.EqualTo(2));
        }


        [Test]
        public void AnswersNumberOfSharesForPurchasedSymbol()
        {
            portfolio.Purchase("IBM", 10);

            Assert.That(portfolio.Shares("IBM"), Is.EqualTo(10));
        }

        [Test]
        public void SeparatesSharesBySymbol()
        {
            portfolio.Purchase("IBM", 10);
            portfolio.Purchase("RELX", 30);

            Assert.That(portfolio.Shares("IBM"), Is.EqualTo(10));
        }

        [Test]
        public void ReturnsZeroForSharesOfUnpurchasedSymbol()
        {
            Assert.That(portfolio.Shares("RELX"), Is.Zero);
        }

        [Test]
        public void SumsSharesPurchasedWithSameSymbol()
        {
            portfolio.Purchase("RELX", 30);
            portfolio.Purchase("RELX", 12);

            Assert.That(portfolio.Shares("RELX"), Is.EqualTo(42));
        }

        [Test]
        public void SellReducesSharesCount()
        {
            portfolio.Purchase("RELX", 30);
            portfolio.Sell("RELX", 12);

            Assert.That(portfolio.Shares("RELX"), Is.EqualTo(18));
        }

        [Test]
        public void ThrowsWhenSellingTooManyShares()
        {
            portfolio.Purchase("RELX", 30);
            Assert.Throws<InvalidTransactionException>(
                () => portfolio.Sell("RELX", 30 + 1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ThrowsWhenPurchasingNonPositiveShares(int shares)
        {
            Assert.Throws<InvalidTransactionException>(
                () => portfolio.Purchase("RELX", shares));
        }

        [Test]
        public void ThrowsWhenSellingSymbolNotPurchased()
        {
            var exception = Assert.Throws<InvalidTransactionException>(
                () => portfolio.Sell("RELX", 10));
            Assert.That(exception.Message, Is.EqualTo("symbol not held"));
        }

        [Test]
        public void ReturnsDateOfLastTransaction()
        {
            var timestamp = new DateTime(2018, 3, 15);
            TimeService.NextTime = timestamp;

            portfolio.Purchase("RELX", 30);

            Assert.That(portfolio.DateOfLastTransaction, Is.EqualTo(timestamp));
        }

        [Test]
        public void AnswersDateOfLastTransactionBySymbol()
        {
            TimeService.NextTime = new DateTime(2018, 1, 1);
            portfolio.Purchase("RELX", 30);
            TimeService.NextTime = new DateTime(2018, 1, 2);
            portfolio.Purchase("IBM", 40);

            Assert.That(portfolio.LastTransactionDate("RELX"), 
                Is.EqualTo(new DateTime(2018, 1, 1)));
        }
    }
}
