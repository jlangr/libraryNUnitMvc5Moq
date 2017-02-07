using System.Net;
using System.Web.Mvc;
using Library.Models;
using Library.Models.Repositories;
using System.Collections.Generic;

namespace Library.Controllers
{
    public class PatronsController : Controller
    {
        IRepository<Patron> patronRepo;
        IRepository<Holding> holdingRepo;

        public PatronsController()
        {
            patronRepo = new EntityRepository<Patron>(db => db.Patrons);
            holdingRepo = new EntityRepository<Holding>(db => db.Holdings);
        }

        public PatronsController(IRepository<Patron> patronRepo, IRepository<Holding> holdingRepo)
        {
            this.patronRepo = patronRepo;
            this.holdingRepo = holdingRepo;
        }

        // GET: Patrons
        public ActionResult Index()
        {
            return View(patronRepo.GetAll());
        }

        // GET: Patrons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patrons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance")] Patron patron)
        {
            if (ModelState.IsValid)
            {
                patronRepo.Create(patron);
                return RedirectToAction("Index");
            }
            return View(patron);
        }

        // GET: Patrons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var patron = patronRepo.GetByID(id.Value);
            if (patron == null)
                return HttpNotFound();
            return View(patron);
        }

        public ActionResult Holdings(int patronId)
        {
            var holdings = holdingRepo.FindBy(holding => holding.HeldByPatronId == patronId);
            return View(holdings);
        }

        // GET: Patrons/Delete/5
        public ActionResult Delete(int? id)
        {
            return Edit(id);
        }

        // GET: Patrons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var patron = patronRepo.GetByID(id.Value);
            if (patron == null)
                return HttpNotFound();
            var patronView = new PatronViewModel(patron);
            patronView.Holdings = new List<Holding> (holdingRepo.FindBy(holding => holding.HeldByPatronId == id));
            return View(patronView);
        }

        // POST: Patrons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Balance")] Patron patron)
        {
            if (ModelState.IsValid)
            {
                patronRepo.Save(patron);
                return RedirectToAction("Index");
            }
            return View(patron);
        }

        // POST: Patrons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            patronRepo.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                patronRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
