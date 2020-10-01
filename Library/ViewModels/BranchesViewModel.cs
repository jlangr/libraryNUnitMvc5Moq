using System.Collections;

namespace Library.Models
{
    public class BranchesViewModel
    {
        public Branch SelectedBranch { get; set; }
        public IEnumerable Branches { get; set; }
    }
}