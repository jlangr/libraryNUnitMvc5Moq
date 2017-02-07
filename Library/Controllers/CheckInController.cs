using System.Collections.Generic;
using Library.Models;
using Library.Models.Repositories;
using System.Web.Mvc;
using Library.Util;
using Library.ControllerHelpers;

namespace Library.Controllers
{
    public class CheckInController: Controller
    {
        public const string ModelKey = "CheckIn";
        private IRepository<Branch> branchRepo;
        private IRepository<Holding> holdingRepo;
        private IRepository<Patron> patronRepo;

        public CheckInController(IRepository<Branch> branchRepo, IRepository<Holding> holdingRepo, IRepository<Patron> patronRepo)
        {
            this.branchRepo = branchRepo;
            this.holdingRepo = holdingRepo;
            this.patronRepo = patronRepo;
        }

        public CheckInController()
        {
            branchRepo = new EntityRepository<Branch>(db => db.Branches);
            holdingRepo = new EntityRepository<Holding>(db => db.Holdings);
            patronRepo = new EntityRepository<Patron>(db => db.Patrons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckInViewModel checkin)
        {
            checkin.BranchesViewList = new List<Branch>(branchRepo.GetAllIncludingCheckedOutBranch());

            if (!Holding.IsBarcodeValid(checkin.Barcode))
            {
                ModelState.AddModelError(ModelKey, "Invalid holding barcode format.");
                return View(checkin);
            }

            var holding = HoldingsControllerUtil.FindByBarcode(holdingRepo, checkin.Barcode);
            if (holding == null)
            {
                ModelState.AddModelError(ModelKey, "Invalid holding barcode.");
                return View(checkin);
            }
            if (!holding.IsCheckedOut)
            {
                ModelState.AddModelError(ModelKey, "Holding is already checked in.");
                return View(checkin);
            }

            holding.CheckIn(TimeService.Now, checkin.BranchId);
            holdingRepo.Save(holding);

            return RedirectToAction("Index");
        }
    }
}