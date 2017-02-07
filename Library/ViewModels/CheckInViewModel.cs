using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class CheckInViewModel
    {
        public string Barcode { get; set; }
        public List<Branch> BranchesViewList { get; internal set; }
        public int BranchId { get; set; }
    }
}