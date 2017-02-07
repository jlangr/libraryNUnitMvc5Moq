using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Library.Util;
using Library.Models;
using Library.Scanner;
using Library.Models.Repositories;
using Library.ControllerHelpers;

namespace LibraryTest.Library.Scanner
{
    [TestFixture]
    public class ScanStationTest
    {
        const string SomeBarcode = "QA123:1";

        readonly DateTime now = DateTime.Now;

        ScanStation scanner;
        IRepository<Holding> holdingRepo;
        IRepository<Patron> patronRepo;
        Mock<IClassificationService> classificationService;
        int somePatronId;

        [SetUp]
        public void Initialize()
        {
            holdingRepo = new InMemoryRepository<Holding>();
            patronRepo = new InMemoryRepository<Patron>();
            classificationService = new Mock<IClassificationService>();
            AlwaysReturnBookMaterial(classificationService);
            somePatronId = patronRepo.Create(new Patron { Name = "x" });

            // calling .Object on a mock for the first time is an expensive operation (~200+ ms)
            scanner = new ScanStation(1, classificationService.Object, holdingRepo, patronRepo);
        }

        void AlwaysReturnBookMaterial(Mock<IClassificationService> classificationService)
        {
            classificationService.Setup(service => service.Retrieve(Moq.It.IsAny<string>()))
                .Returns(new Material() { CheckoutPolicy = new BookCheckoutPolicy() });
        }

        void ScanNewMaterial(string barcode)
        {
            var classification = Holding.ClassificationFromBarcode(barcode);
            var isbn = "x";
            classificationService.Setup(service => service.Classification(isbn)).Returns(classification);
            scanner.AddNewMaterial(isbn);
        }

        Holding GetByBarcode(string barcode)
        {
            return HoldingsControllerUtil.FindByBarcode(holdingRepo, barcode);
        }

        void CheckOut(string barcode)
        {
            CheckOut(barcode, somePatronId);
        }

        void CheckOut(string barcode, int patronId)
        {
            CheckOut(barcode, patronId, now);
        }

        void CheckOut(string barcode, int patronId, DateTime dateTime)
        {
            TimeService.NextTime = dateTime;
            scanner.AcceptLibraryCard(patronId);
            TimeService.NextTime = dateTime;
            scanner.AcceptBarcode(barcode);
        }

        void CheckIn(string barcode)
        {
            CheckIn(barcode, now);
        }

        void CheckIn(string barcode, DateTime dateTime)
        {
            TimeService.NextTime = dateTime;
            scanner.AcceptBarcode(barcode);
        }

        public class TestsNotRequiringCheckout : ScanStationTest
        {
            [Test]
            public void StoresHoldingAtBranchWhenNewMaterialAdded()
            {
                classificationService.Setup(service => service.Classification("anIsbn")).Returns("AB123");

                scanner.AddNewMaterial("anIsbn");

                Assert.That(GetByBarcode("AB123:1").BranchId, Is.EqualTo(scanner.BranchId));
            }

            [Test]
            public void CopyNumberIncrementedWhenNewMaterialWithSameIsbnAdded()
            {
                classificationService.Setup(service => service.Classification("anIsbn")).Returns("AB123");
                scanner.AddNewMaterial("anIsbn");

                var holding = scanner.AddNewMaterial("anIsbn");

                var holdingBarcodes = holdingRepo.GetAll().Select(h => h.Barcode);
                Assert.That(holdingBarcodes, Is.EquivalentTo(new List<string> { "AB123:1", "AB123:2" }));
            }

            [Test]
            public void ThrowsWhenCheckingInCheckedOutBookWithoutPatronScan()
            {
                ScanNewMaterial(SomeBarcode);

                Assert.Throws<CheckoutException>(() => scanner.AcceptBarcode(SomeBarcode));
            }

            [Test]
            public void PatronIdUpdatedWhenLibraryCardAccepted()
            {
                scanner.AcceptLibraryCard(somePatronId);

                Assert.That(scanner.CurrentPatronId, Is.EqualTo(somePatronId));
            }

            [Test]
            public void PatronIdClearedWhenCheckoutCompleted()
            {
                scanner.AcceptLibraryCard(somePatronId);

                scanner.CompleteCheckout();

                Assert.That(scanner.CurrentPatronId, Is.EqualTo(ScanStation.NoPatron));
            }
        }

        public class WhenNewMaterialCheckdOut : ScanStationTest
        {
            [SetUp]
            public void CheckOutNewMaterial()
            {
                ScanNewMaterial(SomeBarcode);
                CheckOut(SomeBarcode);
            }

            [Test]
            public void HeldByPatronIdUpdated()
            {
                Assert.That(GetByBarcode(SomeBarcode).HeldByPatronId, Is.EqualTo(somePatronId));
            }

            [Test]
            public void CheckOutTimestampUpdated()
            {
                Assert.That(GetByBarcode(SomeBarcode).CheckOutTimestamp, Is.EqualTo(now));
            }

            [Test]
            public void IsCheckedOutMarkedTrue()
            {
                Assert.That(GetByBarcode(SomeBarcode).IsCheckedOut, Is.True);
            }

            [Test]
            public void RescanBySamePatronIsIgnored()
            {
                scanner.AcceptBarcode(SomeBarcode);

                Assert.That(GetByBarcode(SomeBarcode).HeldByPatronId, Is.EqualTo(somePatronId));
            }

            [Test]
            public void SecondMaterialCheckedOutAddedToPatron()
            {
                ScanNewMaterial("XX123:1");

                CheckOut("XX123:1");

                Assert.That(GetByBarcode(SomeBarcode).HeldByPatronId, Is.EqualTo(somePatronId));
                Assert.That(GetByBarcode("XX123:1").HeldByPatronId, Is.EqualTo(somePatronId));
            }

            [Test]
            public void SecondPatronCanCheckOutSecondCopyOfSameClassification()
            {
                string barcode1Copy2 = Holding.GenerateBarcode(Holding.ClassificationFromBarcode(SomeBarcode), 2);
                ScanNewMaterial(barcode1Copy2);

                var patronId2 = patronRepo.Create(new Patron());
                scanner.AcceptLibraryCard(patronId2);
                scanner.AcceptBarcode(barcode1Copy2);

                Assert.That(GetByBarcode(barcode1Copy2).HeldByPatronId, Is.EqualTo(patronId2));
            }

            [Test]
            public void CheckInAtSecondBranchResultsInTransfer()
            {
                var newBranchId = scanner.BranchId + 1;
                var scannerBranch2 = new ScanStation(newBranchId, classificationService.Object, holdingRepo, patronRepo);

                scannerBranch2.AcceptBarcode(SomeBarcode);

                Assert.That(GetByBarcode(SomeBarcode).BranchId, Is.EqualTo(newBranchId));
            }

            [Test]
            public void LateCheckInResultsInFine()
            {
                scanner.CompleteCheckout();
                const int daysLate = 2;

                CheckIn(SomeBarcode, DaysPastDueDate(SomeBarcode, now, daysLate));

                Assert.That(patronRepo.GetByID(somePatronId).Balance, 
                    Is.EqualTo(RetrievePolicy(SomeBarcode).FineAmount(daysLate)));
            }

            private CheckoutPolicy RetrievePolicy(string barcode)
            {
                var classification = Holding.ClassificationFromBarcode(barcode);
                var material = classificationService.Object.Retrieve(classification);
                return material.CheckoutPolicy;
            }

            [Test]
            public void CheckoutByOtherPatronSucceeds()
            {
                scanner.CompleteCheckout();
                var anotherPatronId = patronRepo.Create(new Patron());
                scanner.AcceptLibraryCard(anotherPatronId);

                CheckOut(SomeBarcode, anotherPatronId);

                Assert.That(GetByBarcode(SomeBarcode).HeldByPatronId, Is.EqualTo(anotherPatronId));
            }

            [Test]
            public void CheckoutByOtherPatronAssessesAnyFineOnFirst()
            {
                scanner.CompleteCheckout();
                var anotherPatronId = patronRepo.Create(new Patron());

                const int daysLate = 2;
                CheckOut(SomeBarcode, anotherPatronId, DaysPastDueDate(SomeBarcode, now, daysLate));

                Assert.That(patronRepo.GetByID(somePatronId).Balance,
                    Is.EqualTo(RetrievePolicy(SomeBarcode).FineAmount(daysLate)));
            }

            private DateTime DaysPastDueDate(string barcode, DateTime fromDate, int daysLate)
            {
                return now.AddDays(RetrievePolicy(barcode).MaximumCheckoutDays() + daysLate);
            }
        }

        public class WhenMaterialCheckedIn : ScanStationTest
        {
            [SetUp]
            public void CheckOutAndCheckInNewMaterial()
            {
                ScanNewMaterial(SomeBarcode);
                CheckOut(SomeBarcode);
                scanner.CompleteCheckout();
                CheckIn(SomeBarcode);
            }

            [Test]
            public void PatronCleared()
            {
                Assert.That(GetByBarcode(SomeBarcode).HeldByPatronId, Is.EqualTo(Holding.NoPatron));
            }

            [Test]
            public void HoldingMarkedAsNotCheckedOut()
            {
                Assert.That(GetByBarcode(SomeBarcode).IsCheckedOut, Is.False);
            }

            [Test]
            public void CheckOutTimestampCleared()
            {
                Assert.That(GetByBarcode(SomeBarcode).CheckOutTimestamp, Is.Null);
            }

            [Test]
            public void LastCheckedInTimestampUpdated()
            {
                Assert.That(GetByBarcode(SomeBarcode).LastCheckedIn, Is.EqualTo(now));
            }
        }
    }
}