using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class WalletFundingRequest
    {
        public string WalletId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
