using System.Web.Mvc;
using NUnit.Framework;
using Library.Controllers;
using Library.Models;
using Library.Models.Repositories;
using Library.Extensions.SystemWebMvcController;
using System;

namespace LibraryTest.Library.Controllers
{
    [TestFixture]
    public class CheckOutControllerTest
    {
        CheckOutController controller;
        IRepository<Holding> holdingRepo;
        IRepository<Patron> patronRepo;
        CheckOutViewModel checkout;
        int someValidBranchId;
        private int someValidPatronId;
        private Holding aCheckedInHolding;

        [SetUp]
        public void Initialize()
        {
            holdingRepo = new InMemoryRepository<Holding>();

            var branchRepo = new InMemoryRepository<Branch>();
            someValidBranchId = branchRepo.Create(new Branch() { Name = "b" });

            patronRepo = new InMemoryRepository<Patron>();
            someValidPatronId = patronRepo.Create(new Patron { Name = "x" });

            controller = new CheckOutController(branchRepo, holdingRepo, patronRepo);
            checkout = new CheckOutViewModel();
        }

        [SetUp]
        public void CreateCheckedInHolding()
        {
            aCheckedInHolding = new Holding { Classification = "ABC", CopyNumber = 1 };
            aCheckedInHolding.CheckIn(DateTime.Now, someValidBranchId);
        }

        class CheckOutGeneratesError : CheckOutControllerTest
        {
            [Test]
            public void WhenPatronIdInvalid()
            {
                var result = controller.Index(new CheckOutViewModel { PatronId = 0, Barcode = aCheckedInHolding.Barcode }) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckOutController.ModelKey), Is.EqualTo("Invalid patron ID."));
            }

            [Test]
            public void WhenNoHoldingFoundForBarcode()
            {
                var result = controller.Index(new CheckOutViewModel { Barcode = "NONEXISTENT:1", PatronId = someValidPatronId }) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckOutController.ModelKey), Is.EqualTo("Invalid holding barcode."));
            }

            [Test]
            public void WhenHoldingAlreadyCheckedOut()
            {
                holdingRepo.Create(aCheckedInHolding);
                checkout = new CheckOutViewModel { Barcode = aCheckedInHolding.Barcode, PatronId = someValidPatronId };
                controller.Index(checkout);

                var result = controller.Index(checkout) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckOutController.ModelKey), Is.EqualTo("Holding is already checked out."));
            }

            [Test]
            public void WhenBarcodeHasInvalidFormat()
            {
                var result = controller.Index(new CheckOutViewModel { Barcode = "HasNoColon", PatronId = someValidPatronId }) as ViewResult;

                Assert.That(controller.SoleErrorMessage(CheckOutController.ModelKey), Is.EqualTo("Invalid holding barcode format."));
            }
        }

        class WhenCheckoutSucceeds: CheckOutControllerTest
        {
            private int holdingId;

            [SetUp]
            public void CreateSuccessfulCheckout()
            {
                holdingId = holdingRepo.Create(aCheckedInHolding);
                checkout = new CheckOutViewModel { Barcode = aCheckedInHolding.Barcode, PatronId = someValidPatronId };
            }

            [Test]
            public void ThenMarksHoldingAsCheckedOut()
            {
                controller.Index(checkout);

                var retrievedHolding = holdingRepo.GetByID(holdingId);
                Assert.That(retrievedHolding.IsCheckedOut);
            }

            [Test]
            public void ThenRedirectsToIndex()
            {
                var result = controller.Index(checkout) as RedirectToRouteResult;

                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            }
        }
    }
}
