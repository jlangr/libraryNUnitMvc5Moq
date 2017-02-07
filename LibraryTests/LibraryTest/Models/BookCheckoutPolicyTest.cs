using System;
using NUnit.Framework;
using Library.Models;

namespace LibraryTest.Library.Models
{
    [TestFixture]
    public class BookCheckoutPolicyTest
    {
        private CheckoutPolicy policy;

        [SetUp]
        public void Initialize()
        {
            policy = new BookCheckoutPolicy();
        }

        // TODO Use abstract test here and for next test!
        [Test]
        public void NoDaysLateIfReturnedOnTime()
        {
            var checkoutDate = DateTime.Now;
            var checkinDate = checkoutDate.AddDays(policy.MaximumCheckoutDays());
            Assert.That(policy.DaysLate(checkoutDate, checkinDate), Is.Zero);
        }

        [Test]
        public void OneDayLateWhenReturnedDayAfterDue()
        {
            var checkoutDate = DateTime.Now;
            var checkinDate = checkoutDate.AddDays(policy.MaximumCheckoutDays() + 1);
            Assert.That(policy.DaysLate(checkoutDate, checkinDate), Is.EqualTo(1));
        }

        [Test]
        public void FineIsDaysLateTimesBasis()
        {
            var daysLate = 1;
            Assert.That(policy.FineAmount(daysLate++), Is.EqualTo(BookCheckoutPolicy.DailyFineBasis * 1));
            Assert.That(policy.FineAmount(daysLate++), Is.EqualTo(BookCheckoutPolicy.DailyFineBasis * 2));
            Assert.That(policy.FineAmount(daysLate), Is.EqualTo(BookCheckoutPolicy.DailyFineBasis * 3));
        }
    }
}
