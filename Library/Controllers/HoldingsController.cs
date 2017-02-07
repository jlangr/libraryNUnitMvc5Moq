using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Library.Models;
using Library.Models.Repositories;
using Library.ControllerHelpers;

namespace Library.Controllers
{
    public class HoldingsController : Controller
    {
        public const string ModelKey = "Holdings";
        private IRepository<Holding> holdingRepo;
        private IRepository<Branch> branchRepo;

        public HoldingsController()
        {
            holdingRepo = new EntityRepository<Holding>(db => db.Holdings);
            branchRepo = new EntityRepository<Branch>(db => db.Branches);
        }

        public HoldingsController(IRepository<Holding> holdingRepo, IRepository<Branch> branchRepo)
        {
            this.holdingRepo = holdingRepo;
            this.branchRepo = branchRepo;
        }

        // GET: Holdings
        public ActionResult Index()
        {
            var model = holdingRepo.GetAll().Select(
                holding => new HoldingViewModel(holding)
                {
                    BranchName = BranchesControllerUtil.BranchName(branchRepo, holding.BranchId)
                });
            return View(model);
        }

        // GET: Holdings/Details/5
        public ActionResult Details(int? id)
        {
            return Edit(id);
        }

        // GET: Holdings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var holding = holdingRepo.GetByID(id.Value);
            if (holding == null)
                return HttpNotFound();
            return ViewWithBranches(holding);
        }

        private ActionResult ViewWithBranches(Holding holding)
        {
            return View(new HoldingViewModel(holding) { BranchesViewList = new List<Branch>(branchRepo.GetAllIncludingCheckedOutBranch()) });
        }

        // GET: Holdings/Create
        public ActionResult Create()
        {
            return ViewWithBranches(new Holding());
        }

        // POST: Holdings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Classification,CopyNumber,CheckOutTimestamp,BranchId,HeldByPatronId,LastCheckedIn")] Holding holding)
        {
            if (ModelState.IsValid)
            {
                if (holding.CopyNumber == 0)
                    holding.CopyNumber = HoldingsControllerUtil.NextAvailableCopyNumber(holdingRepo, holding.Classification);
                else
                {
                    if (HoldingsControllerUtil.FindByBarcode(holdingRepo, holding.Barcode) != null)
                    {
                        ModelState.AddModelError(ModelKey, "Duplicate classification / copy number combination.");
                        return View(holding);
                    }
                }

                var id = holdingRepo.Create(holding);
                return RedirectToAction("Index", new { ID = id });
            }
            return View(holding);
        }

        // POST: Holdings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Classification,CopyNumber,CheckOutTimestamp,BranchId,HeldByPatronId,LastCheckedIn")] Holding holding)
        {
            if (ModelState.IsValid)
            {
                holdingRepo.Save(holding);
                return RedirectToAction("Index");
            }
            return View(holding);
        }

        // GET: Holdings/Delete/5
        public ActionResult Delete(int? id)
        {
            return Edit(id);
        }

        // POST: Holdings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            holdingRepo.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                holdingRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
