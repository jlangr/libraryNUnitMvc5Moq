using System.Linq;
using Library.Models;
using Library.Models.Repositories;
using NUnit.Framework;
using System.Collections.Generic;

namespace LibraryTests.LibraryTest.Models.Repositories
{
    [TestFixture]
    public class BranchRepositoryExtensionsTest
    {
        [Test]
        public void PrependsCheckedOutBranchToListOfAllBranches()
        {
            var branchRepo = new InMemoryRepository<Branch>();
            branchRepo.Create(new Branch { Name = "A" });
            branchRepo.Create(new Branch { Name = "B" });

            var branches = branchRepo.GetAllIncludingCheckedOutBranch();

            Assert.That(branches.First().Name, Is.EqualTo(Branch.CheckedOutBranch.Name));
            Assert.That(branches.Skip(1).Select(b => b.Name), Is.EquivalentTo(new List<string> { "A", "B" }));
        }
    }
}
