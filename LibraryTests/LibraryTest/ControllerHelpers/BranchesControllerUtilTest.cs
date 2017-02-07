using NUnit.Framework;
using Library.ControllerHelpers;
using Library.Models;
using Library.Models.Repositories;

namespace LibraryTest.Library.ControllerHelpers
{
    public class BranchesControllerUtilTest
    {
        [Test]
        public void BranchNameForCheckedOutBranch()
        {
            Assert.That(BranchesControllerUtil.BranchName(new InMemoryRepository<Branch>(), Branch.CheckedOutId),
                Is.EqualTo(BranchesControllerUtil.CheckedOutBranchName));
        }

        [Test]
        public void BranchNameForBranch()
        {
            var branchRepo = new InMemoryRepository<Branch>();
            var branchId = branchRepo.Create(new Branch { Name = "NewBranchName" });

            var branchName = BranchesControllerUtil.BranchName(branchRepo, branchId);

            Assert.That(branchName, Is.EqualTo("NewBranchName"));
        }
    }
}
