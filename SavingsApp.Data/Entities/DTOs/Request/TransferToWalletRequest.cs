using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class TransferToWalletRequest
    {
        public string SenderWalletId { get; set; }
        public string ReceiverWalletId { get; set; }
        public double Amount { get; set; }
    }
}
