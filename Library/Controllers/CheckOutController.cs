using System.Collections.Generic;
using System.Web.Mvc;
using Library.Models;
using Library.Models.Repositories;
using Library.Util;
using Library.ControllerHelpers;

namespace Library.Controllers
{
    public class CheckOutController : Controller
    {
        public const string ModelKey = "CheckOut";
        private IRepository<Branch> branchRepo;
        private IRepository<Holding> holdingRepo;
        private IRepository<Patron> patronRepo;

        public CheckOutController()
        {
            branchRepo = new EntityRepository<Branch>(db => db.Branches);
            holdingRepo = new EntityRepository<Holding>(db => db.Holdings);
            patronRepo = new EntityRepository<Patron>(db => db.Patrons);
        }

        public CheckOutController(IRepository<Branch> branchRepo, IRepository<Holding> holdingRepo, IRepository<Patron> patronRepo)
        {
            this.branchRepo = branchRepo;
            this.holdingRepo = holdingRepo;
            this.patronRepo = patronRepo;
        }

        // GET: CheckOut
        public ActionResult Index()
        {
            var model = new CheckOutViewModel { BranchesViewList = new List<Branch>(branchRepo.GetAllIncludingCheckedOutBranch()) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckOutViewModel checkout)
        {
            if (!ModelState.IsValid)
                return View(checkout);

            checkout.BranchesViewList = new List<Branch>(branchRepo.GetAllIncludingCheckedOutBranch()); 

            var patron = patronRepo.GetByID(checkout.PatronId);
            if (patron == null)
            {
                ModelState.AddModelError(ModelKey, "Invalid patron ID.");
                return View(checkout);
            }

            if (!Holding.IsBarcodeValid(checkout.Barcode))
            {
                ModelState.AddModelError(ModelKey, "Invalid holding barcode format.");
                return View(checkout);
            }

            var holding = HoldingsControllerUtil.FindByBarcode(holdingRepo, checkout.Barcode);
            if (holding == null)
            {
                ModelState.AddModelError(ModelKey, "Invalid holding barcode.");
                return View(checkout);
            }
            if (holding.IsCheckedOut)
            {
                ModelState.AddModelError(ModelKey, "Holding is already checked out.");
                return View(checkout);
            }

            // TODO determine policy material, which in turn comes from from Isbn lookup on creation 
            // Currently Holding creation in controller does not accept ISBN
            holding.CheckOut(TimeService.Now, checkout.PatronId, new BookCheckoutPolicy());
            holdingRepo.Save(holding);

            return RedirectToAction("Index");
        }
    }
}