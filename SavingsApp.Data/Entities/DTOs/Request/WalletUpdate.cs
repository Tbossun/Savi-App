using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class WalletUpdate
    {
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public double Balance { get; set; }
    }
}
