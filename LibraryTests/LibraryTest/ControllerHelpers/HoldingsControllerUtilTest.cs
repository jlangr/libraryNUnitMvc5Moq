using Library.ControllerHelpers;
using Library.Models;
using Library.Models.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTests.LibraryTest.ControllerHelpers
{
    [TestFixture]
    public class HoldingsControllerUtilTest
    {
        IRepository<Holding> holdingRepo;

        [SetUp]
        public void createRepo()
        {
            holdingRepo = new InMemoryRepository<Holding>();
        }

        class NextAvailableCopyNumber : HoldingsControllerUtilTest
        {
            [Test]
            public void NextAvailableCopyNumberIncrementsCopyNumberUsingCount()
            {
                holdingRepo.Create(new Holding("AB123:1"));
                holdingRepo.Create(new Holding("AB123:2"));
                holdingRepo.Create(new Holding("XX123:1"));

                var copyNumber = HoldingsControllerUtil.NextAvailableCopyNumber(holdingRepo, "AB123");

                Assert.That(copyNumber, Is.EqualTo(3));
            }
        }

        class FindByBarcodeOrBarcodeDetails : HoldingsControllerUtilTest
        {
            int idForAB123_2;
            int idForXX123_1;

            [SetUp]
            public void AddSomeHoldings()
            {
                holdingRepo.Create(new Holding("AB123:1"));
                idForAB123_2 = holdingRepo.Create(new Holding("AB123:2"));
                idForXX123_1 = holdingRepo.Create(new Holding("XX123:1"));
            }

            [Test]
            public void ByBarcodeReturnsMatchingHolding()
            {
                Assert.That(HoldingsControllerUtil.FindByBarcode(holdingRepo, "AB123:2").Id, Is.EqualTo(idForAB123_2));
            }

            [Test]
            public void ByClassificationAndCopyReturnsMatchingHolding()
            {
                Assert.That(HoldingsControllerUtil.FindByClassificationAndCopy(holdingRepo, "XX123", 1).Id, Is.EqualTo(idForXX123_1));
            }
        }
    }
}
