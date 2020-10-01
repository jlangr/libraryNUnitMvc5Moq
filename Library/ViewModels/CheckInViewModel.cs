using System.Collections.Generic;
using Library.Models;

namespace Library.ViewModels
{
    public class CheckInViewModel
    {
        public string Barcode { get; set; }
        public List<Branch> BranchesViewList { get; internal set; }
        public int BranchId { get; set; }
    }
}