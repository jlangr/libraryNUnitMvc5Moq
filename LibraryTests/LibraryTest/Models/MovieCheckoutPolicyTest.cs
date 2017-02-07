using NUnit.Framework;
using Library.Models;

namespace LibraryTest.Library.Models
{
    [TestFixture]
    public class MovieCheckoutPolicyTest
    {
        private CheckoutPolicy policy;

        [SetUp]
        public void Initialize()
        {
            policy = new MovieCheckoutPolicy();
        }

        [Test]
        public void DailyAccumulatingFine()
        {
            var daysLate = 1;
            Assert.That(policy.FineAmount(daysLate++), Is.EqualTo(MovieCheckoutPolicy.PenaltyAmount + MovieCheckoutPolicy.DailyFineBasis * 1));
            Assert.That(policy.FineAmount(daysLate++), Is.EqualTo(MovieCheckoutPolicy.PenaltyAmount + MovieCheckoutPolicy.DailyFineBasis * 2));
            Assert.That(policy.FineAmount(daysLate), Is.EqualTo(MovieCheckoutPolicy.PenaltyAmount + MovieCheckoutPolicy.DailyFineBasis * 3));
        }
    }
}
