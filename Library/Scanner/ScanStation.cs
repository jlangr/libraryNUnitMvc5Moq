using System;
using Library.Util;
using Library.Models;
using Library.Models.Repositories;
using Library.ControllerHelpers;

namespace Library.Scanner
{
    public class ScanStation
    {
        public const int NoPatron = -1;
        private readonly IClassificationService classificationService;
        private readonly int brId;
        private int cur = NoPatron;
        private DateTime cts;
        private IRepository<Patron> patronRepo;
        private IRepository<Holding> holdingRepo;

        public ScanStation(int branchId)
            : this(branchId,
                  new MasterClassificationService(),
                  new EntityRepository<Holding>(db => db.Holdings),
                  new EntityRepository<Patron>(db => db.Patrons))
        {
        }

        public ScanStation(int branchId, IClassificationService classificationService, IRepository<Holding> holdingRepo, IRepository<Patron> patronRepo)
        {
            this.classificationService = classificationService;
            this.holdingRepo = holdingRepo;
            this.patronRepo = patronRepo;
            BranchId = branchId;
            brId = BranchId;
        }

        public Holding AddNewMaterial(string isbn)
        {
            var classification = classificationService.Classification(isbn);
            var holding = new Holding
            {
                Classification = classification,
                CopyNumber = HoldingsControllerUtil.NextAvailableCopyNumber(holdingRepo, classification),
                BranchId = BranchId
            };
            holdingRepo.Create(holding);
            return holding;
        }

        public int BranchId { get; set; }
        public int CurrentPatronId { get { return cur; } }

        public void AcceptLibraryCard(int patronId)
        {
            cur = patronId;
            cts = TimeService.Now;
        }

        // 1/19/2017: who wrote this?
        // 
        // FIXME. Fix this mess. We just have to SHIP IT for nwo!!!
        public void AcceptBarcode(string bc)
        {
            var cl = Holding.ClassificationFromBarcode(bc);
            var cn = Holding.CopyNumberFromBarcode(bc);
            var h = HoldingsControllerUtil.FindByBarcode(holdingRepo, bc);

            if (h.IsCheckedOut)
            {
                if (cur == NoPatron)
                { // ci
                    bc = h.Barcode;
                    var patronId = h.HeldByPatronId;
                    var cis = TimeService.Now;
                    Material m = null;
                    m = classificationService.Retrieve(h.Classification);
                    var fine = m.CheckoutPolicy.FineAmount(h.CheckOutTimestamp.Value, cis);
                    var p = patronRepo.GetByID(patronId);
                    p.Fine(fine);
                    patronRepo.Save(p);
                    h.CheckIn(cis, brId);
                    holdingRepo.Save(h);
                }
                else
                {
                    if (h.HeldByPatronId != cur) // check out book already cked-out
                    {
                        var bc1 = h.Barcode;
                        var n = TimeService.Now;
                        var t = TimeService.Now.AddDays(21);
                        var f = classificationService.Retrieve(h.Classification).CheckoutPolicy.FineAmount(h.CheckOutTimestamp.Value, n);
                        var patron = patronRepo.GetByID(h.HeldByPatronId);
                        patron.Fine(f);
                        patronRepo.Save(patron);
                        h.CheckIn(n, brId);
                        holdingRepo.Save(h);
                        // co
                        h.CheckOut(n, cur, CheckoutPolicies.BookCheckoutPolicy);
                        holdingRepo.Save(h);
                        // call check out controller(cur, bc1);
                        t.AddDays(1);
                        n = t;
                    }
                    else // not checking out book already cked out by other patron
                    {
                        // otherwise ignore, already checked out by this patron
                    }
                }
            }
            else
            {
                if (cur != NoPatron) // check in book
                {
                    h.CheckOut(cts, cur, CheckoutPolicies.BookCheckoutPolicy);
                    holdingRepo.Save(h);
                }
                else
                    throw new CheckoutException();
            }
        }

        public void CompleteCheckout()
        {
            cur = NoPatron;
        }
    }
}
