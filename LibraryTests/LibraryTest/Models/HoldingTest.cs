using System;
using NUnit.Framework;
using Library.Models;

/*
This test class is a mess. Some of the following opportunities for cleanup might exist:

 - AAA used but no visual separation
 - seeming use of AAA but it's not really
 - duplicate and uninteresting initialization
 - unnecessary code (null checks? try/catch?)
 - things that bury information relevant to the test
 - inconsistent test names
 - test names that don't emphasize behavior
 - comments in tests (are they even true)?
 - multiple behaviors/asserts per test
 - dead code
 - local variables that add no readability or other value
 - variables that don't provide enough relevant info
 */


namespace LibraryTest.Library.Models
{
    [TestFixture]
    public class HoldingTest
    {
        const int PatronId = 101;
        const string ExpectedBarcode = "QA234:3";
        const int SomeBranchId = 1;

        [Test]
        public void CreateWithCommonArguments()
        {
            const int branchId = 10;
            var holding = new Holding("QA123", 2, branchId);
            Assert.That(holding, Is.Not.Null);
            Assert.That(holding.Barcode, Is.EqualTo("QA123:2"), "Barcode not concatenated properly");
            Assert.That(holding.BranchId, Is.EqualTo(branchId));
        }

        [Test]
        public void IsValidBarcodeReturnsFalseWhenItHasNoColon()
        {
            Assert.That(Holding.IsBarcodeValid("ABC"), Is.False, "Barcode valid when expected to be false");
        }

        [Test]
        public void IsValidBarcodeReturnsFalseWhenItsCopyNumberNotPositiveInt()
        {
            Assert.That(Holding.IsBarcodeValid("ABC:X"), Is.False);
            Assert.That(Holding.IsBarcodeValid("ABC:0"), Is.False);
        }

        [Test]
        public void IsValidBarcodeReturnsFalseWhenItsClassificationIsEmpty()
        {
            Assert.That(Holding.IsBarcodeValid(":1"), Is.False);
        }

        [Test]
        public void IsValidBarcodeReturnsTrueWhenFormattedCorrectly()
        {
            Assert.That(Holding.IsBarcodeValid("ABC:1"));
        }

        [Test]
        public void GenBarcode()
        {
            Assert.That(Holding.GenerateBarcode("QA234", 3), Is.EqualTo(ExpectedBarcode));
        }

        [Test]
        public void ClassificationFromBarcode()
        {
            try
            {
                Assert.That(Holding.ClassificationFromBarcode(ExpectedBarcode), Is.EqualTo("QA234"));
            }
            catch (FormatException)
            {
                Assert.Fail("fail");
            }
        }

        [Test]
        public void ParsesCopyNoFromBarcode()
        {
            try
            {
                Assert.That(Holding.CopyNumberFromBarcode(ExpectedBarcode), Is.EqualTo(3));
            }
            catch (FormatException)
            {
                Assert.Fail("test threw format exception");
            }
        }

        [Test]
        public void CopyNumberFromBarcodeThrowsWhenNoColonExists()
        {
            Assert.Throws<FormatException>(() => Holding.CopyNumberFromBarcode("QA234"));
        }

        [Test]
        public void Co()
        {
            var holding = new Holding { Classification = "", CopyNumber = 1, BranchId = 1 };
            Assert.IsFalse(holding.IsCheckedOut);
            var now = DateTime.Now;

            var policy = CheckoutPolicies.BookCheckoutPolicy;
            holding.CheckOut(now, PatronId, policy);

            Assert.IsTrue(holding.IsCheckedOut);

            Assert.AreSame(policy, holding.CheckoutPolicy);
            Assert.That(holding.HeldByPatronId, Is.EqualTo(PatronId));

            var dueDate = now.AddDays(policy.MaximumCheckoutDays());
            Assert.That(holding.DueDate, Is.EqualTo(dueDate));

            Assert.That(holding.BranchId, Is.EqualTo(Branch.CheckedOutId));
        }

        [Test]
        public void CheckIn()
        {
            var holding = new Holding { Classification = "X", BranchId = 1, CopyNumber = 1 };
            // check out movie
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);
            var tomorrow = DateTime.Now.AddDays(1);
            const int newBranchId = 2;
            holding.CheckIn(tomorrow, newBranchId);
            Assert.IsFalse(holding.IsCheckedOut);
            Assert.That(holding.HeldByPatronId, Is.EqualTo(Holding.NoPatron));
            Assert.That(holding.CheckOutTimestamp, Is.Null);
            Assert.That(holding.BranchId, Is.EqualTo(newBranchId));
            // day after now
            Assert.That(holding.LastCheckedIn, Is.EqualTo(tomorrow));
        }

        [Test]
        public void CheckInAnswersZeroDaysLateWhenReturnedOnDueDate()
        {
            var holding = new Holding { Classification = "X", BranchId = 1, CopyNumber = 1 };
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            var dueDate = holding.DueDate.Value;
            int brId = 2;
            
            holding.CheckIn(dueDate, brId);
            Assert.That(holding.DaysLate(), Is.EqualTo(0));
        }

        [Test]
        public void DaysLateCalculatedWhenReturnedAfterDueDate()
        {
            var holding = new Holding { Classification = "X", BranchId = 1, CopyNumber = 1 };
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            var date = holding.DueDate.Value.AddDays(2);
            int branchId = 2;
            
            holding.CheckIn(date, branchId);
            Assert.That(holding.DaysLate(), Is.EqualTo(2));
        }

        [Test]
        public void CheckInAnswersZeroDaysLateWhenReturnedBeforeDueDate()
        {
            var holding = new Holding { Classification = "X", BranchId = 1, CopyNumber = 1 };
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            var date = holding.DueDate.Value.AddDays(-1);
            int branchId = 2;
            
            holding.CheckIn(date, branchId);
            Assert.That(holding.DaysLate(), Is.EqualTo(0));
        }
    }
}
