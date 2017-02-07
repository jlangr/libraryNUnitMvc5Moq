using Library.Controllers;
using Library.Models;

namespace Library.ControllerHelpers
{
    public class BranchesControllerUtil
    {
        public const string CheckedOutBranchName = "** checked out **";

        public static string BranchName(IRepository<Branch> branchRepo, int branchId)
        {
            if (branchId == Branch.CheckedOutId)
                return CheckedOutBranchName;
            return branchRepo.GetByID(branchId).Name;
        }
    }
}
