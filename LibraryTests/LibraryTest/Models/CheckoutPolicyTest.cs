using System;
using NUnit.Framework;
using Library.Models;

namespace LibraryTest.Library.Models
{
    [TestFixture]
    public class CheckoutPolicyTest
    {
        [Test]
        public void NotLateIfReturnedWithinMaxDays()
        {
            const int maxCheckoutDays = 5;

            var checkoutDate = DateTime.Now;
            var checkinDate = DateTime.Now.AddDays(maxCheckoutDays);

            Assert.That(new StubCheckoutPolicy().DaysLate(checkoutDate, checkinDate, maxCheckoutDays), Is.Zero);
        }

        [Test]
        public void OneDayLate()
        {
            const int maxCheckoutDays = 5;

            var checkoutDate = DateTime.Now;
            var checkinDate = DateTime.Now.AddDays(maxCheckoutDays + 1);

            Assert.That(new StubCheckoutPolicy().DaysLate(checkoutDate, checkinDate, maxCheckoutDays), Is.EqualTo(1));
        }

        [Test]
        public void ACoupleYearsLate()
        {
            const int maxCheckoutDays = 2;
            var checkoutDate = new DateTime(2017, 1, 1);
            var checkinDate = new DateTime(2019, 1, 1);
            Assert.That(
                new StubCheckoutPolicy().DaysLate(checkoutDate, checkinDate, maxCheckoutDays), Is.EqualTo(365 * 2 - 2));
        }

        [Test]
        public void CalculatesFineFromDaysAndPeriod()
        {
            var policy = new StubCheckoutPolicy();

            const int daysLate = 2;

            var checkoutDate = DateTime.Now;
            var checkinDate = DateTime.Now.AddDays(policy.MaximumCheckoutDays() + daysLate);

            Assert.That(policy.FineAmount(checkoutDate, checkinDate), Is.EqualTo(StubCheckoutPolicy.FixedFine));
            Assert.That(StubCheckoutPolicy.LastDaysLate, Is.EqualTo(daysLate));
        }
    }

    class StubCheckoutPolicy : CheckoutPolicy
    {
        public const decimal FixedFine = 100;
        public static int LastDaysLate;
        
        public override int MaximumCheckoutDays()
        {
            return 10;
        }

        public override decimal FineAmount(int daysLate)
        {
            LastDaysLate = daysLate;
            return FixedFine;
        }
    }
}
