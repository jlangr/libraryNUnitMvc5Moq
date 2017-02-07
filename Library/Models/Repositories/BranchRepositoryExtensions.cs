using System.Collections.Generic;
using System.Linq;

namespace Library.Models.Repositories
{
    public static class BranchRepositoryExtensions
    {
        public static IEnumerable<Branch> GetAllIncludingCheckedOutBranch(this IRepository<Branch> repo)
        {
            return new List<Branch> { Branch.CheckedOutBranch }.Concat(repo.GetAll());
        }
    }
}