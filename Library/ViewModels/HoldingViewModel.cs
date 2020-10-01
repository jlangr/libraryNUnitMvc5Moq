using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Models;

namespace Library.ViewModels
{
    [NotMapped]
    public class HoldingViewModel: Holding
    {
        public HoldingViewModel() { }
        public HoldingViewModel(Holding holding)
        {
            this.Id = holding.Id;
            this.BranchId = holding.BranchId;
            this.CheckoutPolicy = holding.CheckoutPolicy;
            this.CheckOutTimestamp = holding.CheckOutTimestamp;
            this.Classification = holding.Classification;
            this.CopyNumber = holding.CopyNumber;
            this.DueDate = holding.DueDate;
            this.HeldByPatronId = holding.HeldByPatronId;
        }

        [NotMapped, DisplayName("Branch")]
        public string BranchName { get; set; }
    }
}