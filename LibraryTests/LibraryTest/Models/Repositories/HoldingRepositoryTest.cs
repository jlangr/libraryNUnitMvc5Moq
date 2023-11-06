using Library.ControllerHelpers;
using Library.Models;
using Library.Models.Repositories;
using NUnit.Framework;

namespace LibraryTests.Library.Models.Repositories
{
    [TestFixture, Category("slow")]
    public class HoldingRepositoryTest
    {
        InMemoryRepository<Holding> repo;

        [SetUp]
        public void Create()
        {
            repo = new InMemoryRepository<Holding>(); //db => db.Holdings);
            repo.Clear();
        }

        [Test]
        public void FindByBarcodeReturnsNullWhenNotFound()
        {
            Assert.That(HoldingsControllerUtil.FindByBarcode(repo, "AA:1"), Is.Null);
        }

        [Test]
        public void FindByBarcodeReturnsHoldingMatchingClassificationAndCopy()
        {
            var holding = new Holding { Classification = "AA123", CopyNumber = 2 };

            repo.Create(holding);

            Assert.That(HoldingsControllerUtil.FindByBarcode(repo, "AA123:2"), Is.EqualTo(holding));
        }
    }
}
