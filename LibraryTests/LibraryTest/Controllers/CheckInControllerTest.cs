using Library.Controllers;
using Library.Extensions.SystemWebMvcController;
using Library.Models;
using Library.Models.Repositories;
using Library.Util;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace LibraryTest.Library.Controllers
{
    [TestFixture]
    public class CheckInControllerTest
    {
        private InMemoryRepository<Branch> branchRepo;
        private CheckInViewModel checkin;
        private CheckInController controller;
        private InMemoryRepository<Holding> holdingRepo;
        private InMemoryRepository<Patron> patronRepo;
        private int someValidBranchId;
        private int someValidPatronId;

        [SetUp]
        public void Initialize()
        {
            holdingRepo = new InMemoryRepository<Holding>();

            branchRepo = new InMemoryRepository<Branch>();
            someValidBranchId = branchRepo.Create(new Branch() { Name = "b" });

            patronRepo = new InMemoryRepository<Patron>();

            controller = new CheckInController(branchRepo, holdingRepo, patronRepo);
            checkin = new CheckInViewModel();
        }

        class CheckInGeneratesError: CheckInControllerTest
        {
            [Test]
            public void WhenHoldingWithBarcodeDoesNotExist()
            {
                var result = controller.Index(new CheckInViewModel { Barcode = "NONEXISTENT:42", BranchId = someValidBranchId }) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckInController.ModelKey), Is.EqualTo("Invalid holding barcode."));
            }

            [Test]
            public void WhenHoldingBarcodeIsInvalid()
            {
                var result = controller.Index(new CheckInViewModel { Barcode = "BADFORMAT", BranchId = someValidBranchId }) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckInController.ModelKey), Is.EqualTo("Invalid holding barcode format."));
            }

            [Test]
            public void WhenHoldingAlreadyCheckedIn()
            {
                holdingRepo.Create(new Holding { Classification = "X", CopyNumber = 1, BranchId = 1 });

                var result = controller.Index(new CheckInViewModel { Barcode = "X:1", BranchId = someValidBranchId }) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckInController.ModelKey), Is.EqualTo("Holding is already checked in."));
            }
        }

        class WhenCheckInSucceeds : CheckInControllerTest
        {
            private Holding aCheckedOutHolding;
            private DateTime now;

            [SetUp]
            public void CreateCheckedOutHolding()
            {
                var aHoldingId = holdingRepo.Create(new Holding { Classification = "ABC", CopyNumber = 1, BranchId = Branch.CheckedOutId, HeldByPatronId = someValidPatronId });
                aCheckedOutHolding = holdingRepo.GetByID(aHoldingId);
            }

            [SetUp]
            public void CreateValidPatron()
            {
                var someValidPatron = new Patron { Name = "X" };
                someValidPatronId = patronRepo.Create(someValidPatron);
            }

            [SetUp]
            public void FixTimeService()
            {
                now = DateTime.Now;
                TimeService.NextTime = now;
            }

            RedirectToRouteResult CheckInHolding()
            {
                return controller.Index(new CheckInViewModel { Barcode = aCheckedOutHolding.Barcode, BranchId = someValidBranchId }) as RedirectToRouteResult;
            }

            [Test]
            public void ThenHoldingBranchIsUpdated()
            {
                CheckInHolding();

                Assert.That(holdingRepo.GetByID(aCheckedOutHolding.Id).BranchId, Is.EqualTo(someValidBranchId));
            }

            [Test]
            public void ThenHoldingIsNotCheckedOut()
            {
                CheckInHolding();

                Assert.That(holdingRepo.GetByID(aCheckedOutHolding.Id).IsCheckedOut, Is.False);
            }

            [Test]
            public void ThenHoldingPatronIsCleared()
            {
                CheckInHolding();

                Assert.That(holdingRepo.GetByID(aCheckedOutHolding.Id).HeldByPatronId, Is.EqualTo(Holding.NoPatron));
            }

            [Test]
            public void ThenHoldingLastDateCheckedInIsUpdated()
            {
                CheckInHolding();

                Assert.That(holdingRepo.GetByID(aCheckedOutHolding.Id).LastCheckedIn, Is.EqualTo(now));
            }

            [Test]
            public void ThenRedirectsToIndex()
            {
                Assert.That(CheckInHolding().RouteValues["action"], Is.EqualTo("Index"));
            }
        }

        // TODO exercise: fines on late checkin
    }
}