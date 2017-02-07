using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Library.Controllers;
using Library.Models;
using Library.Models.Repositories;

namespace LibraryTest.Library.Controllers
{
    [TestFixture]
    public class PatronsControllerTest
    {
        private PatronsController controller;
        private InMemoryRepository<Patron> patronRepo;
        private InMemoryRepository<Holding> holdingRepo;

        [SetUp]
        public void Initialize()
        {
            patronRepo = new InMemoryRepository<Patron>();
            holdingRepo = new InMemoryRepository<Holding>();
            controller = new PatronsController(patronRepo, holdingRepo);
        }

        public class Details: PatronsControllerTest
        {
            [Test]
            public void ReturnsNotFoundWhenNoPatronAdded()
            {
                var view = controller.Details(0);

                Assert.That(view, Is.TypeOf<HttpNotFoundResult>());
            }

            [Test]
            public void ReturnsBadRequestErrorWhenIdNull()
            {
                var view = controller.Details(null);

                Assert.That((view as HttpStatusCodeResult).StatusCode, Is.EqualTo(400));
            }

            [Test]
            public void ReturnsViewOnPatronWhenFound()
            {
                var id = patronRepo.Create(new Patron() { Name = "Jeff" }); 

                var view = controller.Details(id);

                var viewPatron = (view as ViewResult).Model as Patron;
                Assert.That(viewPatron.Name, Is.EqualTo("Jeff"));
            }
        }

        public class Holdings: PatronsControllerTest
        {
            IRepository<Branch> branchRepo = new InMemoryRepository<Branch>();
            CheckOutController checkoutController;
            int patronId;
            int branchId;

            [SetUp]
            public void CreateCheckoutController()
            {
                checkoutController = new CheckOutController(branchRepo, holdingRepo, patronRepo);
            }

            [SetUp]
            public void CreatePatron()
            {
                patronId = patronRepo.Create(new Patron());
            }

            [SetUp]
            public void CreateBranch()
            {
                branchId = branchRepo.Create(new Branch());
            }

            [Test]
            public void ReturnsEmptyWhenPatronHasNotCheckedOutBooks()
            {
                var view = (controller.Holdings(patronId) as ViewResult).Model as IEnumerable<Holding>;

                Assert.That(!view.Any());
            }

            [Test]
            public void ReturnsListWithCheckedOutHolding()
            {
                int holdingId1 = CreateCheckedOutHolding(patronId, checkoutController, 1);
                int holdingId2 = CreateCheckedOutHolding(patronId, checkoutController, 2);

                var view = (controller.Holdings(patronId) as ViewResult).Model as IEnumerable<Holding>;

                Assert.That(view.Select(h => h.Id), Is.EqualTo(new List<int> { holdingId1, holdingId2 }));
            }

            private int CreateCheckedOutHolding(int id, CheckOutController checkoutController, int copyNumber)
            {
                var holdingId = holdingRepo.Create(new Holding { Classification = "X", CopyNumber = copyNumber, BranchId = branchId });
                var checkOutViewModel = new CheckOutViewModel { Barcode = $"X:{copyNumber}", PatronId = id };
                var result = checkoutController.Index(checkOutViewModel) as ViewResult;
                return holdingId;
            }
        }
        
        public class Index: PatronsControllerTest
        {
            [Test]
            public void RetrievesViewOnAllPatrons()
            {
                patronRepo.Create(new Patron { Name = "Alpha" }); 
                patronRepo.Create(new Patron { Name = "Beta" }); 

                var view = controller.Index();

                var patrons = (view as ViewResult).Model as IEnumerable<Patron>;
                Assert.That(patrons.Select(p => p.Name), 
                    Is.EqualTo(new string[] { "Alpha", "Beta" }));
            }
        }

        public class Create: PatronsControllerTest
        {
            [Test]
            public void CreatesPatronWhenModelStateValid()
            {
                var patron = new Patron { Name = "Venkat" };

                controller.Create(patron);

                var retrieved = patronRepo.GetAll().First();
                Assert.That(retrieved.Name, Is.EqualTo("Venkat"));
            }

            [Test]
            public void RedirectsToIndexWhenModelValid()
            {
                var result = controller.Create(new Patron()) as RedirectToRouteResult;

                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            }

            [Test]
            public void AddsNoPatronWhenModelStateInvalid()
            {
                controller.ModelState.AddModelError("", "");

                controller.Create(new Patron());

                Assert.False(patronRepo.GetAll().Any());
            }
        }
    }
}
